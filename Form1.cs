using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System.IO;
using System.Configuration;
using System.Windows.Forms.VisualStyles;
using System.Runtime.CompilerServices;

namespace ProbaZaMongoDB
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            if(textPath.Text == "")
            {
                MessageBox.Show("Please select a path!");
            }
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            var database = client.GetDatabase("Proba");


            var bucket = new GridFSBucket(database);
            string filename = textName.Text;
            FileStream source = new FileStream(textPath.Text , FileMode.Open);
/*            MemoryStream source = new MemoryStream(Encoding.UTF8.GetBytes(@textPath.Text));*/
            var id = bucket.UploadFromStream(filename, source);
        }

        private void Download_Click(object sender, EventArgs e)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            var database = client.GetDatabase("Proba");

            var bucket = new GridFSBucket(database);

            string filename = "Dnd uputstvo za mene.odt";

            string where = ("D:\\MongoProba\\" + filename);

            using (var newFs = new FileStream(where, FileMode.Create))
            {
                var t1 = bucket.DownloadToStreamByNameAsync(filename, newFs);
                Task.WaitAll(t1);
                newFs.Flush();
                newFs.Close();
            }
        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textPath.Text = ofd.FileName;
                    textName.Text = ofd.SafeFileName;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

