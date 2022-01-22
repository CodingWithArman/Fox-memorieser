using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Drawing;

namespace Phonebook
{

    class jarvis
    {

        string connection = (@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source ='" + (Application.StartupPath + "/database\\fx_memo.accdb") + "';Jet OLEDB:Database Password=mintoriam;");
        OleDbConnection con;
        OleDbCommand cmd;
        DataTable dt;
        DataSet ds;
        OleDbDataAdapter sda;

        public void watermark(string txt, Label lbl)
        {
            if (txt == "")
            {
                lbl.Show();
            }
            else if (txt != "")
            {
                lbl.Hide();
            }

        }
        public void focus(Label lbl, TextBox txt)
        {
            txt.Focus();
        }
        public void query(string s1)
        {

            con = new OleDbConnection(connection);
            //string query = "" + s1 + "";
            cmd = new OleDbCommand(s1, con);
        }

        public string execute()
        {
            string msg;
            try
            {
             con.Open();
            cmd.ExecuteNonQuery();
                msg = "Command is run successfuly";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                msg = "Status: Command is Fail";
            }
            finally
            {
            con.Close();
               
            }
          return msg;



        }

        public string login_method(string sql_query,string column_name)
        {
            string msg="";
         try
            {
                query(sql_query);
                sda = new OleDbDataAdapter(cmd);      
                DataTable dt = new DataTable(); 
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {                  
                    msg =  dt.Rows[0][column_name].ToString();                 
                }
                else
                {
                msg = "false";
                }                
            }
            catch
            {
                msg = "false";
            }
            return msg;              
        }
            
        public void Bind_Grid(DataGridView mGrid,/*TextBox txt_count_row,*/string s)
        {

            query(s);
            sda = new OleDbDataAdapter(cmd);
            dt = new DataTable(); con.Open();
            sda.Fill(dt);
            if (dt.Rows.Count == 0)
            {

            }
            if (dt.Rows.Count != 0)
            {
                mGrid.DataSource = dt;
             

            }
            con.Close();


        }
        public string profile_path(string sql_query,string column_name)
        {
            //"SELECT user_profile FROM user_mst WHERE [user_code] =@user_code"

            string result;
            query(sql_query);
            sda = new OleDbDataAdapter(cmd);
            dt = new DataTable();  con.Open();
            sda.Fill(dt);

            result = dt.Rows[0][column_name].ToString();
            return result;

        }
         public string profile_image(PictureBox pxbox)
        {
            //string profile_path="";
           string profile_path = "/Files/default.png";
            OpenFileDialog opFile = new OpenFileDialog();
            opFile.Title = "Select a Image";
            opFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

            string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Files\"; // <---
            if (Directory.Exists(appPath) == false)                                              // <---
            {                                                                                    // <---
                Directory.CreateDirectory(appPath);                                              // <---
            }                                                                                    // <---

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string iName = opFile.SafeFileName;   // <---
                    string filepath = opFile.FileName;    // <---

                    // <---
                    if (File.Exists(appPath + iName) == false)                                              // <---
                    {
                        File.Copy(filepath, appPath + iName);
                        pxbox.Image = new Bitmap(opFile.OpenFile());
                        profile_path = "\\Files\\" + iName;
                    }
                    else if (File.Exists(appPath + iName) == true)
                    {
                        pxbox.Image = Image.FromFile(Application.StartupPath + "\\Files\\" + iName);
                        profile_path = "\\Files\\" + iName;
                    }


                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file " + exp.Message);
                }
            }
            else
            {
                opFile.Dispose();
            }
        
            return profile_path;

        }

        public string Find_Max(string sql_query)
        {
            string result = "0";
            query(sql_query);
            cmd.Connection.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    result = dr["max_sn"].ToString();
                    if(result=="")
                    {
                        result = "1";
                    }
                
                }
           
            }
            return "00"+result;

        }
        public void remove_grid_old_data(DataGridView mygrid)
        {
            int a = 0;
            int b = 5999;
            for (a = 0; a <= b; a++)
            {
                for (int i = 0; i < mygrid.Rows.Count; i++)
                {
                    mygrid.Rows.RemoveAt(i);
                }
                for (int i = 0; i < mygrid.Rows.Count; i++)
                {
                    mygrid.Rows.RemoveAt(i);
                }
                for (int i = 0; i < mygrid.Rows.Count; i++)
                {
                    mygrid.Rows.RemoveAt(i);
                }
            }
        }
    }
}
