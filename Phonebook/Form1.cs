using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phonebook
{
    public partial class Form1 : Form
    {
        jarvis jar = new jarvis();
        string profile_path;
        
        //string user_profile_path;
        string connection = (@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source ='" + (Application.StartupPath + "/database\\fx_memo.accdb") + "';Jet OLEDB:Database Password=mintoriam;");
        OleDbConnection con;
        OleDbCommand cmd;
        DataTable dt;
        DataSet ds;
        OleDbDataAdapter sda;
        string true_or_false;
        public Form1()
        {
            InitializeComponent();
        }
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 34;   // Caption bar height;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.Indigo, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

           
               sp_con.Panel1Collapsed = true;
            //option_account.Visible = false;
            //option_developer.Visible = false;
            contact_cmb_categeory.SelectedIndex =0;
            create_ac_win_profile.Cursor = Cursors.Hand;
            sp_con.Panel2.BackgroundImage = Image.FromFile(Application.StartupPath + "//images//bg-lg.png");
            profile_path = "/Files/default.png";
            create_ac_win_profile.Image = Image.FromFile(Application.StartupPath + profile_path);
            contact_profile.Image = Image.FromFile(Application.StartupPath + profile_path);
            //CONTENT IMAGE START
            string backimg="";
            string backimage = "//images//banner.jpg";

            if (backimg=="True")
            {
                content_ftp_and_sql.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
                content_note.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
                content_task.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
                content_contacts.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
                content_link.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
                content_user_and_password.BackgroundImage = Image.FromFile(Application.StartupPath + backimage);
            }
            //END
            //CONTENT BACK COLOR START
            string backclr = "True";
            var color = "Black";
            if (backclr == "True")
            {
                content_contacts.BackColor = Color.FromName(color);
                content_link.BackColor = Color.FromName(color);
                content_ftp_and_sql.BackColor = Color.FromName(color);
                content_note.BackColor = Color.FromName(color);
                content_user_and_password.BackColor = Color.FromName(color);
                content_task.BackColor = Color.FromName(color);
            }
            //END
            
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, usr_img.Width, usr_img.Height);
            this.usr_img.Region = new Region(path);
            change_pass_win.Location = new Point(404, 128);   


        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            true_or_false = jar.login_method("select count(*) ,user_profile from user_mst where user_name='" + txtuser.Text + "' and password_='" + txtpass.Text + "' group by user_profile", "user_profile");
             if (true_or_false != "false")
            {
                 login_win.Visible = false;
                
                sp_con.Panel1Collapsed = false;

                //this.bg.Location =this.label2.Location;
                sp_con.Panel2.BackgroundImage = Image.FromFile(Application.StartupPath + "//images//bg-home.jpg");
               usr_img.Image= Image.FromFile(Application.StartupPath + true_or_false);
                lbl_user_name.Text=txtuser.Text;
                
            }         
            else if(true_or_false == "false")
            {
                status_info.Text = "please enter right user and password";
            }
        }

        private void txtuser_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(txtuser.Text, placeholder_user);

        }
        private void txtpass_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(txtpass.Text, placeholder_pass);
        }

        private void placeholder_user_Click(object sender, EventArgs e)
        {
            txtuser.Focus();
        }

        private void placeholder_pass_Click(object sender, EventArgs e)
        {
            txtpass.Focus();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_close_MouseHover(object sender, EventArgs e)
        {
            btn_close.BackColor = System.Drawing.Color.Red;
        }

        private void btn_close_MouseLeave(object sender, EventArgs e)
        {
            btn_close.BackColor = System.Drawing.Color.Black;
        }

        private void btn_contact_Click(object sender, EventArgs e)
        {
            content_note.Dock = DockStyle.None;
            content_contacts.Dock = DockStyle.Fill;
            content_task.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
       
        }

        private void contact_close_Click(object sender, EventArgs e)
        {
            content_contacts.Dock = DockStyle.None;
            content_contacts.Size = new Size(0,0);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            content_note.Dock = DockStyle.None;
            content_contacts.Dock = DockStyle.None;
            content_task.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
                    
            sp_con.Panel1Collapsed = true;

            sp_con.Panel2.BackgroundImage = Image.FromFile(Application.StartupPath + "//images//bg-lg.png");
            login_win.Visible = true;
            contact_data_view();
        }
        public void contact_data_view()
        {
            try
            {
                jar.remove_grid_old_data(contact_dgv);
                con = new OleDbConnection(connection);
                //string query = "" + s1 + "";
                cmd = new OleDbCommand("select s_no, profile_path, name, nick_name, phone, email, categeory, address, description from contact where user_='" + lbl_user_name.Text + "' ", con);
                sda = new OleDbDataAdapter(cmd);
                dt = new DataTable(); con.Open();
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    status_info.Text = "No Data Found!";
                }
                if (dt.Rows.Count != 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i = i + 1)
                    {
                        int row = contact_dgv.RowCount;
                        contact_dgv.Rows.Add();

                        string img = (Application.StartupPath + dt.Rows[i]["profile_path"].ToString());
                        contact_dgv.Rows[row].Cells["Profile"].Value = Image.FromFile(img);
                        contact_dgv.Rows[row].Cells["Name_"].Value = dt.Rows[i]["name"].ToString();
                        contact_dgv.Rows[row].Cells["Nick_Name"].Value = dt.Rows[i]["Nick_name"].ToString();
                        contact_dgv.Rows[row].Cells["Phone_No"].Value = dt.Rows[i]["phone"].ToString();
                        contact_dgv.Rows[row].Cells["Email"].Value = dt.Rows[i]["email"].ToString();
                        contact_dgv.Rows[row].Cells["Address"].Value = dt.Rows[i]["address"].ToString();
                        contact_dgv.Rows[row].Cells["Categeory"].Value = dt.Rows[i]["categeory"].ToString();
                        contact_dgv.Rows[row].Cells["Description"].Value = dt.Rows[i]["description"].ToString();
                        contact_dgv.Rows[row].Cells["id"].Value = dt.Rows[i]["s_no"].ToString();

                    }

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_developer_Click(object sender, EventArgs e)
        { 
   
        }

        private void btn_account_Click(object sender, EventArgs e)
        {
           
        }

        private void contact_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_description, contact_txt_description);
        }
        private void contact_txt_search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                jar.remove_grid_old_data(contact_dgv);
                jar.watermark(contact_txt_search.Text, contact_lbl_search);
                //jar.Bind_Grid(contact_dgv,");
                con = new OleDbConnection(connection);
                //string query = "" + s1 + "";
                cmd = new OleDbCommand("select s_no, profile_path, name, nick_name, phone, email, categeory, address, description from contact where s_no like '%" + contact_txt_search + "%' or name like '%" + contact_txt_search.Text + "%' or nick_name like '%" + contact_txt_search.Text + "%' or phone like'%" + contact_txt_search.Text + "%' or email like '%" + contact_txt_search.Text + "%' or categeory like '%" + contact_txt_search.Text + "%' or address like '%" + contact_txt_search.Text + "%' or description like '%" + contact_txt_search.Text + "%' and user_ = '" + lbl_user_name.Text + "' ", con);
                sda = new OleDbDataAdapter(cmd);
                dt = new DataTable(); con.Open();
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    status_info.Text = "No Data Found!";
                }
                if (dt.Rows.Count != 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i = i + 1)
                    {
                        int row = contact_dgv.RowCount;
                        contact_dgv.Rows.Add();

                        string img = (Application.StartupPath + dt.Rows[i]["profile_path"].ToString());
                        contact_dgv.Rows[row].Cells["Profile"].Value = Image.FromFile(img);
                        contact_dgv.Rows[row].Cells["Name_"].Value = dt.Rows[i]["name"].ToString();
                        contact_dgv.Rows[row].Cells["Nick_Name"].Value = dt.Rows[i]["Nick_name"].ToString();
                        contact_dgv.Rows[row].Cells["Phone_No"].Value = dt.Rows[i]["phone"].ToString();
                        contact_dgv.Rows[row].Cells["Email"].Value = dt.Rows[i]["email"].ToString();
                        contact_dgv.Rows[row].Cells["Address"].Value = dt.Rows[i]["address"].ToString();
                        contact_dgv.Rows[row].Cells["Categeory"].Value = dt.Rows[i]["categeory"].ToString();
                        contact_dgv.Rows[row].Cells["Description"].Value = dt.Rows[i]["description"].ToString();
                        contact_dgv.Rows[row].Cells["id"].Value = dt.Rows[i]["s_no"].ToString();
                    }

                }
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contact_lbl_search_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_search, contact_txt_search);
        }
        private void contact_txt_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_name.Text, contact_lbl_name);
        }

        private void contact_txt_phone_no_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_phone_no.Text, contact_lbl_phono_no);
        }

        private void contact_txt_nick_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_nick_name.Text, contact_lbl_nike_name);
        }

        private void contact_txt_email_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_email.Text, contact_lbl_email);
        }

        private void contact_txt_address_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_address.Text, contact_lbl_address);
        }

        private void contact_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(contact_txt_description.Text, contact_lbl_description);
        }

        private void contact_lbl_name_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_name, contact_txt_name);
        }

        private void contact_lbl_phono_no_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_phono_no, contact_txt_phone_no);
        }

        private void contact_lbl_nike_name_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_nike_name, contact_txt_nick_name);
        }

        private void contact_lbl_email_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_email, contact_txt_email);
        }

        private void contact_lbl_address_Click(object sender, EventArgs e)
        {
            jar.focus(contact_lbl_address, contact_txt_address);
        }

        private void lbl_soft_name_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void content_task_Paint(object sender, PaintEventArgs e)
        {

        }

        private void task_btn_save_Click(object sender, EventArgs e)
        {

        }

        private void task_txt_search_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(task_txt_search.Text, task_lbl_search);
        }

        private void task_lbl_name_Click(object sender, EventArgs e)
        {
            jar.focus(task_lbl_task_name, task_txt_task_name);
        }

        private void task_txt_task_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(task_txt_task_name.Text, task_lbl_task_name);

        }

        private void task_lbl_task_Click(object sender, EventArgs e)
        {
            jar.focus(task_lbl_task, task_txt_task);

        }

        private void task_txt_task_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(task_txt_task.Text, task_lbl_task);

        }

        private void task_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(task_lbl_description, task_txt_description);

        }


        private void task_lbl_search_Click(object sender, EventArgs e)
        {
            jar.focus(task_lbl_search, task_txt_search);

        }

        private void task_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(task_txt_description.Text, task_lbl_description);

        }

        private void task_btn_close_Click(object sender, EventArgs e)
        {
            content_task.Dock = DockStyle.None;
        }

        private void btn_task_Click(object sender, EventArgs e)
        {
            content_task.Dock = DockStyle.Fill;
            content_contacts.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
        }

        private void btn_note_Click(object sender, EventArgs e)
        {
            content_task.Dock = DockStyle.None;
            content_contacts.Dock = DockStyle.None;
            content_note.Dock = DockStyle.Fill;
            content_link.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
        }

        private void note_lbl_search_Click(object sender, EventArgs e)
        {
            jar.focus(note_lbl_search, note_txt_search);
        }

        private void note_txt_note_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(note_txt_note_name.Text, note_lbl_note_name);
        }

        private void note_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(note_lbl_description, note_txt_description);
        }

        private void note_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(note_txt_description.Text, note_lbl_description);
        }

        private void note_lbl_note_Click(object sender, EventArgs e)
        {
            jar.focus(note_lbl_note, note_txt_note);
        }

        private void note_txt_note_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(note_txt_note.Text, note_lbl_note);
        }

        private void note_txt_search_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(note_txt_search.Text, note_lbl_search);
        }

        private void note_lbl_note_name_Click(object sender, EventArgs e)
        {
            jar.focus(note_lbl_note_name, note_txt_note_name);
        }

        private void note_btn_close_Click(object sender, EventArgs e)
        {
            content_note.Dock = DockStyle.None;
        }

        private void ftp_and_sql_lbl_search_Click(object sender, EventArgs e)
        {
            jar.focus(ftp_and_sql_lbl_search, ftp_and_sql_txt_search);
        }

        private void ftp_and_sql_btn_close_Click(object sender, EventArgs e)
        {
            content_ftp_and_sql.Dock = DockStyle.None;
        }

        private void ftp_and_sql_lbl_host_Click(object sender, EventArgs e)
        {
            jar.focus(ftp_and_sql_lbl_host, ftp_and_sql_txt_host);
        }

        private void ftp_and_sql_txt_host_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(ftp_and_sql_txt_host.Text, ftp_and_sql_lbl_host);
        }

        private void ftp_and_sql_lbl_username_Click(object sender, EventArgs e)
        {
            jar.focus(ftp_and_sql_lbl_username, ftp_and_sql_txt_username);
        }

        private void ftp_and_sql_txt_username_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(ftp_and_sql_txt_username.Text, ftp_and_sql_lbl_username);
        }

        private void ftp_and_sql_password_Click(object sender, EventArgs e)
        {
            jar.focus(ftp_and_sql_lbl_password, ftp_and_sql_txt_password);
        }

        private void ftp_and_sql_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(ftp_and_sql_txt_description.Text, ftp_and_sql_lbl_description);
        }

        private void ftp_and_sql_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(ftp_and_sql_lbl_description, ftp_and_sql_txt_description);
        }

        private void ftp_and_sql_txt_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(ftp_and_sql_txt_password.Text, ftp_and_sql_lbl_password);
        }

        private void btn_sql_ftp_Click(object sender, EventArgs e)
        {
            content_task.Dock = DockStyle.None;
            content_contacts.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.Fill;
            content_link.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;

        }

        private void link_txt_search_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(link_txt_search.Text, link_lbl_search);
        }

        private void link_lbl_link_name_Click(object sender, EventArgs e)
        {
            jar.focus(link_lbl_link_name, link_txt_link_name);
        }

        private void link_txt_link_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(link_txt_link_name.Text, link_lbl_link_name);
        }

        private void link_lbl_link_Click(object sender, EventArgs e)
        {
            jar.focus(link_lbl_link, link_txt_link);
        }

        private void link_txt_link_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(link_txt_link.Text, link_lbl_link);
        }

        private void link_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(link_lbl_description, link_txt_description);
        }

        private void link_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(link_txt_description.Text, link_lbl_description);
        }

        private void link_lbl_search_TextChanged(object sender, EventArgs e)
        {
            jar.focus(link_lbl_search, link_txt_search);
        }

        private void link_btn_close_Click(object sender, EventArgs e)
        {
            content_link.Dock = DockStyle.None;
        }

        private void btn_link_Click(object sender, EventArgs e)
        {
            content_link.Dock = DockStyle.Fill;
            content_contacts.Dock = DockStyle.None;
            content_task.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
        }

        private void btn_chng_pass_Click(object sender, EventArgs e)
        {
            change_pass_win_profile.Image = Image.FromFile(Application.StartupPath + true_or_false);
            change_pass_win.Visible = true;
            sp_con.Panel1Collapsed = true;
            sp_con.Panel2.BackgroundImage = Image.FromFile(Application.StartupPath + "//images//bg-lg.png");
            content_contacts.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_task.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
            
        }

        private void change_pass_win_btn_cancel_Click(object sender, EventArgs e)
        {
            change_pass_win.Visible = false;
            login_win.Visible = true;
        }

        private void login_win_lbl_creat_account_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            change_pass_win.Visible = false;
            sp_con.Panel1Collapsed = true;
            create_an_account_win.Dock = DockStyle.Fill;
            sp_con.Panel2.BackgroundImage = Image.FromFile(Application.StartupPath + "//images//bg-lg.png");
            content_contacts.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_task.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.None;
        }


       //START CREATE AN ACCOUND WIN 
        private void create_ac_win_submit_Click(object sender, EventArgs e) 
            {
            if (create_ac_win_txt_confirm_password.Text.Trim()== create_ac_win_txt_password.Text.Trim())
            {
           string DOB = create_ac_win_day.Text + create_ac_win_month.Text + create_ac_win_year.Text;
           string max_user_code= jar.Find_Max("select MAX(user_code)+1 as max_sn from user_mst");
            jar.query("INSERT into user_mst(user_code,user_name,email_id,user_profile,first_name,last_name,password_,DOB,Q1_ANS,Q2_ANS,Q3_ANS,ent_date)values('"+max_user_code+"','"+create_ac_win_txt_user_name.Text+"','"+create_ac_win_txt_email_id.Text+"','" + profile_path + "','" + create_ac_win_txt_first_name.Text + "','" + create_ac_win_txt_last_name.Text + "','" + create_ac_win_txt_password.Text + "','" + DOB + "','" + create_ac_win_txt_Q1.Text + "','" + create_ac_win_txt_Q2.Text + "','" + create_ac_win_txt_Q3.Text + "', Date() );");
           status_info.Text= jar.execute();
                profile_path = "/Files/default.png";
            }
          
                
            //user_profile_path = jar.profile_path("select user_profile from user_mst where user_code='"+create_ac_win_day.Text+"'","user_profile");
            //create_ac_win_profile.Image = Image.FromFile(Application.StartupPath +user_profile_path);

        }

        private void create_ac_win_btn_cancel_Click(object sender, EventArgs e)
        {
            create_an_account_win.Dock = DockStyle.None;
            login_win.Visible = true;
        }
        //END
        private void forget_pass_win_btn_cancel_Click(object sender, EventArgs e)
        {
            forget_password_win.Dock = DockStyle.None;
            login_win.Visible = true;
        }

        private void login_win_lbl_forget_pass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forget_password_win.Dock = DockStyle.Fill;
            login_win.Visible = false;
        }
        private void user_pass_txt_description_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(user_pass_txt_description.Text, user_pass_lbl_description);
        }

        private void user_pass_txt_user_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(user_pass_txt_user.Text, user_pass_lbl_user);
        }

        private void user_pass_lbl_password_Click(object sender, EventArgs e)
        {
            jar.focus(user_pass_lbl_password, user_pass_txt_password);
        }

        private void user_pass_txt_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(user_pass_txt_password.Text, user_pass_lbl_password);
        }

        private void user_pass_lbl_user_Click(object sender, EventArgs e)
        {
            jar.focus(user_pass_lbl_user, user_pass_txt_user);
        }

        private void user_pass_lbl_description_Click(object sender, EventArgs e)
        {
            jar.focus(user_pass_lbl_description, user_pass_txt_description);
        }

        private void user_pass_btn_close_Click(object sender, EventArgs e)
        {
            content_user_and_password.Dock = DockStyle.None;
        }

        private void btn_user_password_Click(object sender, EventArgs e)
        {
            content_task.Dock = DockStyle.None;
            content_contacts.Dock = DockStyle.None;
            content_note.Dock = DockStyle.None;
            content_ftp_and_sql.Dock = DockStyle.None;
            content_link.Dock = DockStyle.None;
            content_user_and_password.Dock = DockStyle.Fill;
        }

        private void forget_pass_win_txt_confirm_pass_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(forget_pass_win_txt_confirm_pass.Text, forget_pass_win_lbl_confirm_pass);
        }

        private void forget_pass_win_txt_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(forget_pass_win_txt_password.Text, forget_pass_win_lbl_pass);
        }

        private void forget_pass_win_lbl_confirm_pass_Click(object sender, EventArgs e)
        {
            jar.focus(forget_pass_win_lbl_confirm_pass, forget_pass_win_txt_confirm_pass);
        }

        private void forget_pass_win_lbl_pass_Click(object sender, EventArgs e)
        {
            jar.focus(forget_pass_win_lbl_pass, forget_pass_win_txt_password);
        }

        private void user_pass_lbl_search_Click(object sender, EventArgs e)
        {
            jar.focus(user_pass_lbl_search, user_pass_txt_search);
        }

        private void user_pass_txt_search_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(user_pass_txt_search.Text, user_pass_lbl_search);
        }

        private void create_ac_win_lbl_first_name_Click(object sender, EventArgs e)
        {
            jar.focus(create_ac_win_lbl_first_name, create_ac_win_txt_first_name);
        }

        private void create_ac_win_lbl_confirm_password_Click(object sender, EventArgs e)
        {
            jar.focus(create_ac_win_lbl_confirm_password, create_ac_win_txt_confirm_password);
        }

        private void create_ac_win_lbl_last_name_Click(object sender, EventArgs e)
        {
            jar.focus(create_ac_win_lbl_last_name, create_ac_win_txt_last_name);
        }

        private void create_ac_win_txt_confirm_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_confirm_password.Text, create_ac_win_lbl_confirm_password);

        }

        private void create_ac_win_txt_last_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_last_name.Text, create_ac_win_lbl_last_name);
        }

        private void create_ac_win_txt_first_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_first_name.Text, create_ac_win_lbl_first_name);
        }

        private void create_ac_win_lbl_password_Click(object sender, EventArgs e)
        {
            jar.focus(create_ac_win_lbl_password, create_ac_win_txt_password);
        }

        private void create_ac_win_txt_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_password.Text, create_ac_win_lbl_password);
        }

        private void sp_con_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_developer_MouseHover(object sender, EventArgs e)
        {
            btn_developer.BackColor= Color.Azure;
        }

        private void btn_developer_MouseLeave(object sender, EventArgs e)
        {
            btn_developer.BackColor = Color.Black;

        }
        
        private void create_ac_win_profile_Click(object sender, EventArgs e)
        {
            profile_path=jar.profile_image(create_ac_win_profile);
        }

        private void create_ac_win_lbl_email_id_Click(object sender, EventArgs e)
        {
jar.focus(create_ac_win_lbl_email_id, create_ac_win_txt_email_id);
        }

        private void create_ac_win_lbl_user_name_Click(object sender, EventArgs e)
        {
                        jar.focus(create_ac_win_lbl_user_name,create_ac_win_txt_user_name);

        }

        private void create_ac_win_txt_user_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_user_name.Text, create_ac_win_lbl_user_name);
        }
        private void create_ac_win_txt_email_id_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(create_ac_win_txt_email_id.Text, create_ac_win_lbl_email_id);
        }

        private void forget_pass_win_btn_submit_Click(object sender, EventArgs e)
        {
            if (forget_pass_win_txt_user_name.TextLength!=0 || forget_pass_txt_Q1.TextLength != 0 || forget_pass_txt_Q2.TextLength != 0 || forget_pass_txt_Q3.TextLength != 0 || forget_pass_win_txt_password.TextLength!=0 || forget_pass_win_txt_confirm_pass.TextLength!=0 || forget_pass_win_txt_password.Text.Trim()== forget_pass_win_txt_confirm_pass.Text.Trim())
            {
                forget_password_check("select user_name,Q1_ANS,Q2_ANS,Q3_ANS FROM user_mst", "user_name", "Q1_ANS", "Q2_ANS", "Q3_ANS");
                if (forget_pass_txt_Q1.Text.Trim() != txt_q1.Trim())
                {
                    status_info.Text = "Your Question 1 Answer is Wrong.";
                }
                else if (forget_pass_win_txt_user_name.Text.Trim() != txt_user_name.Trim())
                {
                    status_info.Text = "Your user Name is Wrong.";

                }
                else if (forget_pass_txt_Q2.Text.Trim() != txt_q2.Trim())
                {
                    status_info.Text = "Your Question 2 Answer is Wrong.";
                }
                else if (forget_pass_txt_Q3.Text.Trim() != txt_q3.Trim())
                {
                    status_info.Text = "Your Question 3 Answer is Wrong.";
                }
              
                else
                {
                    jar.query("update user_mst set password_='"+forget_pass_win_txt_password.Text+"'");
                    status_info.Text = "Password Update Successfuly";
                  status_info.Text = jar.execute();
                }
            }
            else
            {
                status_info.Text = "Please fill the all fields correctly";
            }
        }
        string txt_user_name; string txt_q1; string txt_q2; string txt_q3;
        public void forget_password_check(string sql_query, string column_name, string column_name2, string column_name3, string column_name4)
        {
            //"SELECT user_profile FROM user_mst WHERE [user_code] =@user_code"
            
            con = new OleDbConnection(connection);
            //string query = "" + s1 + "";
            cmd = new OleDbCommand(sql_query, con);
          
            sda = new OleDbDataAdapter(cmd);
            dt = new DataTable(); con.Open();
            sda.Fill(dt);
            txt_user_name = dt.Rows[0][column_name].ToString();
            txt_q1 = dt.Rows[0][column_name2].ToString();
            txt_q2 = dt.Rows[0][column_name3].ToString();
            txt_q3 = dt.Rows[0][column_name4].ToString();
            //return result;
        }
        private void forget_pass_win_txt_user_name_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(forget_pass_win_txt_user_name.Text, forget_pass_win_lbl_user_name);
        }

        private void forget_pass_win_lbl_user_name_Click(object sender, EventArgs e)
        {
            jar.focus(forget_pass_win_lbl_user_name, forget_pass_win_txt_user_name);
        }

        private void change_pass_win_profile_Click(object sender, EventArgs e)
        {
           
        }

        private void change_pass_btn_submit_Click(object sender, EventArgs e)
        {
            true_or_false = jar.login_method("select count(*) ,user_profile from user_mst where user_name='" + lbl_user_name.Text + "' and password_='" + change_pass_win_txt_password.Text + "' group by user_profile", "user_profile");
            if (true_or_false != "false")
            {
                if (change_pass_win_txt_new_password.Text.Trim() == change_pass_win_txt_confirm_password.Text.Trim())
                {
                    jar.query("update user_mst set password_='" + change_pass_win_txt_confirm_password.Text + "' where user_name='" + lbl_user_name.Text + "'");
                    status_info.Text=jar.execute();

                }
            }
           else if(true_or_false == "false")
            {
                status_info.Text = "your old password is wrong";
            }
        }

        private void contact_btn_save_Click(object sender, EventArgs e)
        {
          
                string sn_no = jar.Find_Max("select max(s_no)+1 as max_sn from contact");
                
            jar.query("insert into contact (s_no,profile_path,name,nick_name,phone,email,categeory,address,description,ent_date,user_) values('" + sn_no + "','" + profile_path + "','" + contact_txt_name.Text + "','" + contact_txt_nick_name.Text + "','" + contact_txt_phone_no.Text + "','" + contact_txt_email.Text + "','" + contact_cmb_categeory.Text + "','" + contact_txt_address.Text + "','" + contact_txt_description.Text + "', Date(),'"+lbl_user_name.Text+"')");
                status_info.Text = jar.execute();
           
        }

        private void change_pass_win_lbl_pass_Click(object sender, EventArgs e)
        {
            jar.focus(change_pass_win_lbl_pass, change_pass_win_txt_password);
        }

        private void change_pass_win_lbl_new_password_Click(object sender, EventArgs e)
        {
            jar.focus(change_pass_win_lbl_new_password, change_pass_win_txt_new_password);
        }

        private void change_pass_win_lbl_confirm_password_Click(object sender, EventArgs e)
        {
            jar.focus(change_pass_win_lbl_confirm_password, change_pass_win_txt_confirm_password);
        }

        private void change_pass_win_txt_password_TextChanged(object sender, EventArgs e)
        {
        }

        private void change_pass_win_txt_new_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(change_pass_win_txt_new_password.Text, change_pass_win_lbl_new_password);

        }

        private void change_pass_win_txt_confirm_password_TextChanged(object sender, EventArgs e)
        {
            jar.watermark(change_pass_win_txt_confirm_password.Text, change_pass_win_lbl_confirm_password);

        }

        private void lbl_user_name_Click(object sender, EventArgs e)
        {

        }

        private void contact_profile_Click(object sender, EventArgs e)
        { 
            profile_path=jar.profile_image(contact_profile);
        }

        private void contact_dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { string id;
            if (e.ColumnIndex == 0)
            {
                contact_data_view();
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
                            //pxbox.Image = new Bitmap(opFile.OpenFile());
                            profile_path = "\\Files\\" + iName;
                            contact_dgv.Rows[e.RowIndex].Cells["Profile"].Value = Image.FromFile(Application.StartupPath+profile_path);

                        }
                        else if (File.Exists(appPath + iName) == true)
                        {
                            //pxbox.Image = Image.FromFile(Application.StartupPath + "\\Files\\" + iName);
                            profile_path = "\\Files\\" + iName;
                            contact_dgv.Rows[e.RowIndex].Cells["Profile"].Value = Image.FromFile(Application.StartupPath+profile_path);

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
            }
            if (e.ColumnIndex == 8)
            {
                contact_data_view();
                
                //string Profile=contact_dgv.Rows[e.RowIndex].Cells["Profile"].Value.ToString();
                string Name_ = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Name_"].Value);
                string Nick_Name = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Nick_Name"].Value);
                string Phone_no = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Phone_No"].Value);
                string Email = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Email"].Value);
                string Address = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Address"].Value);
                string Categeory = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Categeory"].Value);
                string Description = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["Description"].Value);

                id = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["id"].Value);

                jar.query("update contact set name='"+Name_+"',nick_name='"+Nick_Name+"',phone='"+Phone_no+"',email='"+Email+"',address='"+Address+"',categeory='"+Categeory+"',description='"+Description+"' where s_no='" + id + "'");
                status_info.Text = jar.execute();
                contact_dgv.Refresh();
            }
                //delete column
                if (e.ColumnIndex == 9)
                {
                contact_data_view();
                    id = Convert.ToString(contact_dgv.Rows[e.RowIndex].Cells["id"].Value);
                    jar.query("delete from contact where s_no = '" + id + "'");
                status_info.Text=jar.execute();              
                    contact_dgv.Refresh();
                }
            }

        private void contact_btn_refresh_Click(object sender, EventArgs e)
        {
            contact_dgv.Refresh();
        }
        //private void btn_developer_MouseHover_1(object sender, EventArgs e)
        //{
        //    btn_developer.BackColor = Color.Azure;
        //}
    }
}
