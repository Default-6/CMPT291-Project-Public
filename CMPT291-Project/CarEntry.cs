﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace CMPT291_Project
{
    public partial class CarEntry : Form
    {
        public SqlConnection myConnection;
        public SqlCommand myCommand;
        public SqlDataReader myReader;
        public int carTypeId, branchId, state = 0;
        public string newCommand, carTypeDes, branchDes;

        public CarEntry()
        {
            InitializeComponent();
            AddRBtn.Checked = true;
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlConnection myConnection = new SqlConnection(connectionString);

            try
            {
                myConnection.Open(); // Open connection
                myCommand = new SqlCommand();
                myCommand.Connection = myConnection; // Link the command stream to the connection
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                this.Close();
            }

            //fill carType combo box
            myCommand.CommandText = "select Description from CarType";
            myReader = myCommand.ExecuteReader();

            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    CarTypePicker.Items.Add(myReader[0]);
                }
            }

            else
            {
                MessageBox.Show("Error Loading Car Types", "Error");
            }

            myReader.Close();

            //fill branch Id combo box
            myCommand.CommandText = "select Description from Branch";
            myReader = myCommand.ExecuteReader();

            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    BranchPicker.Items.Add(myReader[0]);
                }
            }

            else
            {
                MessageBox.Show("Error Loading Branches", "Error");
            }

            myReader.Close();
        }


        private void resetEditRemove()
        {
            FindID.Visible = true;
            vinentry.Text = String.Empty;

            vinentry.Visible = true;
            CarTypePicker.Visible = false;
            makeentry.Visible = false;
            modelentry.Visible = false;
            yearentry.Visible = false;
            seatsentry.Visible = false;
            colourentry.Visible = false;
            insentry.Visible = false;
            mileentry.Visible = false;
            BranchPicker.Visible = false;
        }

        private void resetAddSearch()
        {
            FindID.Visible = false;

            vinentry.Visible = true;
            CarTypePicker.Visible = true;
            makeentry.Visible = true;
            modelentry.Visible = true;
            yearentry.Visible = true;
            seatsentry.Visible = true;
            colourentry.Visible = true;
            insentry.Visible = true;
            mileentry.Visible = true;
            BranchPicker.Visible = true;

            vinentry.Text = String.Empty;
            makeentry.Text = String.Empty;
            modelentry.Text = String.Empty;
            yearentry.Text = String.Empty;
            seatsentry.Text = String.Empty;
            colourentry.Text = String.Empty;
            insentry.Text = String.Empty;
            mileentry.Text = String.Empty;
        }

        private void carentrycancel_Click(object sender, EventArgs e)
        {
            state = 0;
            this.Close();
        }

        private void carentrryaccept_Click_1(object sender, EventArgs e)
        {
            if (AddRBtn.Checked == true)
            {
                
                state = 1;
                if (vinentry.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a VIN.", "Error");
                    return;
                }

                if (int.TryParse(seatsentry.Text, out int checkNum) && int.TryParse(mileentry.Text, out checkNum) && int.TryParse(yearentry.Text, out checkNum))
                {
                    if (BranchPicker.Text.Length > 0 && CarTypePicker.Text.Length > 0)
                    {

                        newCommand = "insert into Car values ('" + vinentry.Text +
                        "'," + carTypeId + ",'" + toCase(makeentry.Text) + "','" + toCase(modelentry.Text) + "'," + yearentry.Text + ","
                        + seatsentry.Text + ",'" + toCase(colourentry.Text) + "','" + insentry.Text + "'," + mileentry.Text + ","
                        + branchId + ")";
                        this.Close();
                    }

                    else //one or both branch and carType are empty
                        MessageBox.Show("Ensure Branch and Car Type have been chosen.", "Error");
                }

                else
                    MessageBox.Show("Ensure Year, Seats, & Mileage are numeric.", "Error");
            }

            else if (EditRBtn.Checked == true)
            {
                state = 1;

                if (vinentry.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a VIN.", "Error");
                    return;
                }

                if (int.TryParse(seatsentry.Text, out int checkNum) && int.TryParse(mileentry.Text, out checkNum) && int.TryParse(yearentry.Text, out checkNum))
                {
                    if (BranchPicker.Text.Length > 0 && CarTypePicker.Text.Length > 0)
                    {
                            newCommand = "update Car set CarTypeId = " + carTypeId + ", Make = '" + toCase(makeentry.Text) + "', Model = '" + toCase(modelentry.Text) + "', Year = "
                        + yearentry.Text + ", Seats = " + seatsentry.Text + ", Colour = '" + toCase(colourentry.Text) + "', Insurance = '" + insentry.Text + "', Odometer = " + mileentry.Text + ", BranchId = "
                        + branchId + "where VIN = '" + vinentry.Text + "'";
                            this.Close();
                    }

                    else //one or both branch and carType are empty
                        MessageBox.Show("Ensure Branch and Car Type have been chosen.", "Error");
                }

               else
                    MessageBox.Show("Ensure Year, Seats, & Mileage are numeric.", "Error");
            }

            else if (RemoveRBtn.Checked == true)
            {
                state = 1;

                if (vinentry.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a VIN.", "Error");
                    return;
                }

                newCommand = "delete from Car where VIN = '" + vinentry.Text + "'";
                this.Close();
            }

            else if (SearchRBtn.Checked == true)
            {
                int first = 1;
                state = 2;
                newCommand = "select * from Car where ";
                if(vinentry.Text != "")
                {
                    first = 0;
                    newCommand += "(VIN like '%" + vinentry.Text + "' or VIN like '" + vinentry.Text + "%')";
                }
                if(makeentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "(Make like '%" + makeentry.Text + "' or Make like '" + makeentry.Text + "%')";
                }
                if (modelentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "(Model like '%" + modelentry.Text + "' or Model like '" + modelentry.Text + "%')";
                }
                if (yearentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "Year = " + yearentry.Text;
                }
                if (seatsentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "Seats = " + seatsentry.Text;
                }
                if (colourentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "(Colour like '%" + colourentry.Text + "' or Colour like '" + colourentry.Text + "%')";
                }
                if (insentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "(Insurance like '%" + insentry.Text + "' or Insurance like '" + insentry.Text + "%')";
                }
                if (mileentry.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "Odometer = " + mileentry.Text;
                }
                if (BranchPicker.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "BranchId = " + BranchPicker.Text;
                }
                if (CarTypePicker.Text != "")
                {
                    if (first != 1)
                        newCommand += " and ";
                    else
                        first = 0;
                    newCommand += "CarTypeId = " + CarTypePicker.Text;
                }
                if (first == 1)
                    newCommand = "select * from Car";
                this.Close();
            }
        }

        private void EditRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            resetEditRemove();
        }

        private void RemoveRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            resetEditRemove();
        }


        private void SearchRBtn_CheckedChanged(object sender, EventArgs e)
        {
            resetAddSearch();
        }

        private void AddRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            resetAddSearch();
        }

        private void FindID_Click_1(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "select * from Car where VIN = '" + vinentry.Text + "'";
                myReader = myCommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    //saves variables read and displays them in the appropriate fields
                    while (myReader.Read())
                    {

                        string vin = (string)myReader["VIN"];
                        int type = (int)myReader["CarTypeId"];
                        string mk = (string)myReader["Make"];
                        string mdl = (string)myReader["Model"];
                        int year = (int)myReader["Year"];
                        int seat = (int)myReader["Seats"];
                        string clr = (string)myReader["Colour"];
                        string ins = (string)myReader["Insurance"];
                        int miles = (int)myReader["Odometer"];
                        carTypeId = (int)myReader["CarTypeId"];
                        branchId = (int)myReader["BranchId"];


                        CarTypePicker.Visible = true;
                        makeentry.Visible = true;
                        modelentry.Visible = true;
                        yearentry.Visible = true;
                        seatsentry.Visible = true;
                        colourentry.Visible = true;
                        insentry.Visible = true;
                        mileentry.Visible = true;
                        BranchPicker.Visible = true;

                        vinentry.Text = vin;
                        makeentry.Text = mk;
                        modelentry.Text = mdl;
                        yearentry.Text = year.ToString();
                        seatsentry.Text = seat.ToString();
                        colourentry.Text = clr;
                        insentry.Text = ins.ToString();
                        mileentry.Text = miles.ToString();

                        myReader.Close();

                        getCarTypeDes();
                        CarTypePicker.SelectedIndex = CarTypePicker.FindString(carTypeDes);

                        getBranchDes();
                        BranchPicker.SelectedIndex = BranchPicker.FindString(branchDes);

                    }
                }

                else
                {
                    vinentry.Text = string.Empty;

                    MessageBox.Show("Invalid VIN", "Error");
                }

            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }
        }

        private void BranchPicker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "select BranchId from Branch where Description = '" + BranchPicker.Text + "'";
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader.HasRows)
                    {
                        branchId = (int)myReader["BranchId"];
                    }
                }
            }

            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }

            myReader.Close();
        }


        private void CarTypePicker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                myCommand.CommandText = "select CarTypeId from CarType where Description = '" + CarTypePicker.Text + "'";
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader.HasRows)
                    {
                        carTypeId = (int)myReader["CarTypeId"];
                    }
                }
            }

            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }

            myReader.Close();
        }

        void getCarTypeDes()
        {
            try
            {
                myCommand.CommandText = "select Description from CarType where CarTypeId = " + carTypeId;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader.HasRows)
                        carTypeDes = (string)myReader["Description"];
                }
            }

            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }

            myReader.Close();
        }
        string toCase(string theString)
        {
            theString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(theString.ToLower());
            return theString;
        }

        void getBranchDes()
        {
            try
            {
                myCommand.CommandText = "select Description from Branch where BranchId = " + branchId;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader.HasRows)
                    {
                        branchDes = (string)myReader["Description"];
                    }
                }
            }

            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString(), "Error");
            }

            myReader.Close();
        }
    }
}
