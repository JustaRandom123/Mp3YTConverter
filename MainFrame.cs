using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using MetroFramework;
using NAudio;
using NAudio.Wave;

namespace Mp3YTConverter
{
    public partial class MainFrame : MetroFramework.Forms.MetroForm
    {
        BackgroundWorker bw;   
        public MainFrame()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Mp3YTConverter.Properties.Settings.Default.downloadPath == "" || string.IsNullOrWhiteSpace(Mp3YTConverter.Properties.Settings.Default.downloadPath))
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select the path where the files should be downloaded to";
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Mp3YTConverter.Properties.Settings.Default.downloadPath = fbd.SelectedPath;
                        Mp3YTConverter.Properties.Settings.Default.Save();
                    }
                }
            }          
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


     

        public void panelCreater(string titel,int videolength,long videocapacity,string url,string audioBitrate,string audioFormat, string videoID)
        {
            Font titelFont = new Font("Arial", 9.75f);
            Font generalFont = new Font("Arial", 8.25f);


           


            var minutes = new TimeSpan(0, 0, videolength).Minutes.ToString();
            var seconds = new TimeSpan(0, 0, videolength).Seconds.ToString();


        



            MetroFramework.Controls.MetroPanel header = new MetroFramework.Controls.MetroPanel();
            header.Style = MetroColorStyle.Silver;
            header.Theme = MetroThemeStyle.Dark;
            header.Size = new Size(1009, 74);

 

            PictureBox videoImage = new PictureBox();
            videoImage.Size = new Size(108, 68);
            videoImage.Location = new Point(3,3);
            videoImage.ImageLocation = "https://img.youtube.com/vi/" + videoID + "/hqdefault.jpg";
            videoImage.SizeMode = PictureBoxSizeMode.StretchImage;


            Label videoLength = new Label();
            videoLength.Text = minutes + ":" + seconds;
            videoLength.Size = new Size(30, 14);
            videoLength.Location = new Point(8, 53);
            videoLength.ForeColor = Color.White;
            videoLength.Font = new Font(generalFont, FontStyle.Bold);
            videoImage.Controls.Add(videoLength);




            Label videoTitel = new Label();
            videoTitel.Text = titel;
            videoTitel.Location = new Point(128, 15);
            videoTitel.Size = new Size(700, 15);
            videoTitel.ForeColor = Color.White;
            videoTitel.Font = new Font(titelFont, FontStyle.Bold);


            Label videoSize = new Label();
            videoSize.Text = ConvertToSize(videocapacity);
            videoSize.Location = new Point(128, 36);
            videoSize.Size = new Size(70,15);
            videoSize.ForeColor= Color.White;  
            videoSize.Font = new Font(generalFont, FontStyle.Bold);


            Label bitrate = new Label();
            bitrate.Text = audioBitrate + " kbs";
            bitrate.Location = new Point(200, 36);
            bitrate.Size = new Size(46, 14);
            bitrate.ForeColor = Color.White;
            bitrate.Font = new Font(generalFont, FontStyle.Bold);



            Label status = new Label();
            status.Text = "Status: ";
            status.Location = new Point(584, 36);
            status.Size = new Size(125, 14);
            status.ForeColor = Color.White;
            status.Font = new Font(generalFont, FontStyle.Bold);
            status.Tag = "statusLabel";



            Label convertTo = new Label();
            convertTo.Text = "Convert to";
            convertTo.Location = new Point(773, 29);
            convertTo.Size = new Size(75, 16);
            convertTo.ForeColor = Color.White;
            convertTo.Font = new Font(titelFont, FontStyle.Bold);


            MetroFramework.Controls.MetroComboBox comboBox = new MetroFramework.Controls.MetroComboBox();
            comboBox.Style = MetroColorStyle.Silver;
            comboBox.Theme = MetroThemeStyle.Dark;
            comboBox.FontSize = MetroComboBoxSize.Small;         
            comboBox.Size = new Size(57, 25);
            comboBox.Location = new Point(851, 26);
            comboBox.Items.Add("mp3");
            comboBox.Items.Add("mp4");
            comboBox.Items.Add("avi");
            comboBox.SelectedIndex = 0;
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
              


            Label audioEx = new Label();
            audioEx.Text = "Format: " + audioFormat;
            audioEx.Location = new Point(252, 36);
            audioEx.Size = new Size(100, 14);
            audioEx.ForeColor = Color.White;
            audioEx.Font = new Font(generalFont, FontStyle.Bold);



            MetroFramework.Controls.MetroProgressBar progressBar = new MetroFramework.Controls.MetroProgressBar();
            progressBar.Value = 0;
            progressBar.Style = MetroColorStyle.Green;
            progressBar.Theme = MetroThemeStyle.Dark;
            progressBar.Size = new Size(875, 18); //875; 18
            progressBar.Location = new Point(131, 53);
            progressBar.HideProgressText = false;


            PictureBox downloadImage = new PictureBox();
            downloadImage.Size = new Size(24,24);
            downloadImage.Location = new Point(935, 26);
            downloadImage.Image = Mp3YTConverter.Properties.Resources.download;
            downloadImage.Click += DownloadImage_Click;
            downloadImage.Cursor = Cursors.Hand;
            downloadImage.Tag = url;


            PictureBox deleteImage = new PictureBox();
            deleteImage.Size = new Size(24, 24);
            deleteImage.Location = new Point(976, 3);
            deleteImage.Image = Mp3YTConverter.Properties.Resources.delete1;
            deleteImage.Click += DeleteImage_Click; ;
            deleteImage.Cursor = Cursors.Hand;
            deleteImage.Tag = header;




            header.Controls.Add(videoImage);
            header.Controls.Add(progressBar);
            header.Controls.Add(videoTitel);
            header.Controls.Add(downloadImage);
            header.Controls.Add(deleteImage);
            header.Controls.Add(videoSize);
            header.Controls.Add(audioEx);
            header.Controls.Add(bitrate);
            header.Controls.Add(comboBox);
            header.Controls.Add(convertTo);
            header.Controls.Add(status);
          



            flowLayoutPanel1.Controls.Add(header);
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  throw new NotImplementedException();
        }

        static string ConvertToSize(long videolength)
        {
            string value = "";
            long totalbytes = videolength / 1024 / 1024;
            long totalbytesKB = videolength / 1024;    
            long totalbytesGB = videolength / 1024 / 1024 / 1024;
          

            if (videolength >= 999)
            {
                value = totalbytes.ToString() + " MB ";
            }
            else if (videolength < 999)
            {             
                value = totalbytesKB.ToString() + " KB ";
            }
            else if (videolength >= 9999)
            {     
                value = totalbytesGB.ToString() + " GB ";
            }

            return value;
        }

        //static int CountChars(string value)
        //{
        //    int result = 0;
        //    bool lastWasSpace = false;

        //    foreach (char c in value)
        //    {
        //        if (char.IsWhiteSpace(c))
        //        {                    
        //            if (lastWasSpace == false)
        //            {
        //                result++;
        //            }
        //            lastWasSpace = true;
        //        }
        //        else
        //        {                 
        //            result++;
        //            lastWasSpace = false;
        //        }
        //    }
        //    return result;
        //}

        private void DeleteImage_Click(object sender, EventArgs e)
        {         
            PictureBox pictureBox = sender as PictureBox;
            Control mainCTRL = pictureBox.Tag as Control;
            BackgroundWorker bgw = mainCTRL.Tag as BackgroundWorker;
            if (bgw != null)
            {
                bgw.CancelAsync();
            }           
            flowLayoutPanel1.Controls.Remove(mainCTRL);
        }

        private void DownloadImage_Click(object sender, EventArgs e)
        {
            List<object> arguments = new List<object>();
            PictureBox clicked = sender as PictureBox;
            clicked.Visible = false;
        
 

            var video = VideoLibrary.YouTube.Default.GetVideo(clicked.Tag.ToString());      
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);


            arguments.Add(video);
            arguments.Add(clicked);
            arguments.Add(getComboBox(clicked));


            bw.RunWorkerAsync(arguments);


            getHeaderPanel(clicked).Tag = bw;



           // getProgessBarNeedToUpdate(clicked);

        }


        private MetroFramework.Controls.MetroProgressBar getProgessBarNeedToUpdate(Control clickedDownlodButton)
        {
            MetroFramework.Controls.MetroProgressBar main = null;
            foreach (MetroFramework.Controls.MetroPanel header in flowLayoutPanel1.Controls)
            {
                if (header.Controls.Contains(clickedDownlodButton))
                {
                    foreach (Control ctrl in header.Controls)
                    {
                        if (ctrl.ToString().Contains("MetroProgressBar"))
                        {
                            main = ctrl as MetroFramework.Controls.MetroProgressBar;
                            break;
                        }
                    }
                    break;
                }
            }
            return main;
        }



        private MetroFramework.Controls.MetroComboBox getComboBox(Control clickedDownlodButton)
        {
            MetroFramework.Controls.MetroComboBox main = null;
            foreach (MetroFramework.Controls.MetroPanel header in flowLayoutPanel1.Controls)
            {
                if (header.Controls.Contains(clickedDownlodButton))
                {
                    foreach (Control ctrl in header.Controls)
                    {
                        if (ctrl.ToString().Contains("MetroComboBox"))
                        {
                            main = ctrl as MetroFramework.Controls.MetroComboBox;
                            break;
                        }
                    }
                    break;
                }
            }
            return main;
        }


        private MetroFramework.Controls.MetroPanel getHeaderPanel(Control clickedDownlodButton)
        {
            MetroFramework.Controls.MetroPanel main = null;
            foreach (MetroFramework.Controls.MetroPanel header in flowLayoutPanel1.Controls)
            {
                if (header.Controls.Contains(clickedDownlodButton))
                {                   
                    main = header;                                         
                    break;
                }
            }
            return main;
        }


        private Label getStatusLabel(Control clickedDownlodButton)
        {
            Label main = null;
            foreach (MetroFramework.Controls.MetroPanel header in flowLayoutPanel1.Controls)
            {
                if (header.Controls.Contains(clickedDownlodButton))
                {
                    foreach (Control ctrl in header.Controls)
                    {
                        if (ctrl.ToString().Contains("Label") && ctrl.Tag != null && ctrl.Tag.ToString() == "statusLabel")
                        {             
                            main = ctrl as Label;
                            break;
                        }
                    }
                    break;
                }
            }
            return main;
        }


        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }




   
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {     
            List<object> argumentsList = e.Argument as List<object>;
            var video = (VideoLibrary.YouTubeVideo)argumentsList[0];
            var comboBox = (MetroFramework.Controls.MetroComboBox)argumentsList[2];
            var clickedDownloadButton = (PictureBox)argumentsList[1];

            getStatusLabel(clickedDownloadButton).Invoke(new MethodInvoker(delegate () { getStatusLabel(clickedDownloadButton).Text = "Status: Downloading"; }));


            byte[] bytes = video.GetBytes();

            try
            {

                using (var writer = new BinaryWriter(System.IO.File.Open(Mp3YTConverter.Properties.Settings.Default.downloadPath + "\\" + video.FullName, FileMode.Create)))
                {
                    var bytesLeft = bytes.Length;
                    var bytesWritten = 0;
                    while (bytesLeft > 0)
                    {
                        int chunk = Math.Min(64, bytesLeft);

                        writer.Write(bytes, bytesWritten, chunk);
                        bytesWritten += chunk;
                        bytesLeft -= chunk;
                        

                        try
                        {
                            getProgessBarNeedToUpdate((Control)argumentsList[1]).Invoke(new MethodInvoker(delegate () { getProgessBarNeedToUpdate((Control)argumentsList[1]).Value = bytesWritten * 100 / bytes.Length; }));
                        }
                        catch
                        {
                            writer.Dispose();
                            writer.Close();
                        }


                        if (bytesWritten * 100 / bytes.Length >= 100)
                        {

                            getStatusLabel(clickedDownloadButton).Invoke(new MethodInvoker(delegate () { getStatusLabel(clickedDownloadButton).Text = "Status: Converting"; }));

                            clickedDownloadButton.Invoke(new MethodInvoker(delegate ()
                            {
                                clickedDownloadButton.Visible = true;
                            }));

                            var selectedFormat = (string)comboBox.Invoke((Func<string>)delegate { return comboBox.SelectedItem.ToString(); });

                                          
                           

                            writer.Dispose();
                            writer.Close();
                            e.Cancel = true;

                            try
                            {
                                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                                ffMpeg.ConvertMedia(Mp3YTConverter.Properties.Settings.Default.downloadPath + "\\" + video.FullName, Mp3YTConverter.Properties.Settings.Default.downloadPath + "\\" + video.FullName.Replace("mp4", selectedFormat), selectedFormat);
                                File.Delete(Mp3YTConverter.Properties.Settings.Default.downloadPath + "\\" + video.FullName);

                                getStatusLabel(clickedDownloadButton).Invoke(new MethodInvoker(delegate () { getStatusLabel(clickedDownloadButton).Text = "Status: Done!"; }));
                                Console.WriteLine("Done! Converting to " + selectedFormat);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }


                           

                        }
                    }
                }
            }
            catch { }
        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            var video = YouTube.getVideoInformations(metroTextBox1.Text);
            panelCreater(video.Info.Title, video.Info.LengthSeconds.Value, video.ContentLength.Value, metroTextBox1.Text, video.AudioBitrate.ToString(),video.AudioFormat.ToString(), getVideoIdByURL(metroTextBox1.Text));
        }


        private string getVideoIdByURL(string url)
        {
            var uri = new Uri(url);          
            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = string.Empty;
       

    

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

       
            return videoId;
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
