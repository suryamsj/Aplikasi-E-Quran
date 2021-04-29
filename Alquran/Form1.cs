using Alquran.connection;
using Alquran.model;
using Alquran.url;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alquran
{
    public partial class Form1 : Form
    {
        bool waiting = false;
        AutoResetEvent stop = new AutoResetEvent(false);
        string link;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IRequestWeb httpWebRequestHandler = new RequestWeb();

            //Manggil Data Quran
            var respon = GetDataQuran(httpWebRequestHandler);
            var data = JsonConvert.DeserializeObject<List<DataQuran>>(respon);
            foreach(var quran in data)
            {
                int outdex = dataGridView1.Rows.Add();
                dataGridView1.Rows[outdex].Cells[0].Value = quran.Nomor;
                dataGridView1.Rows[outdex].Cells[1].Value = quran.Nama;
                dataGridView1.Rows[outdex].Cells[2].Value = quran.Latin;
                dataGridView1.Rows[outdex].Cells[3].Value = quran.Jumlah;
                dataGridView1.Rows[outdex].Cells[4].Value = quran.Deskripsi;
                dataGridView1.Rows[outdex].Cells[5].Value = quran.Tempat;
                dataGridView1.Rows[outdex].Cells[6].Value = quran.Arti;
                dataGridView1.Rows[outdex].Cells[7].Value = quran.Audio;
            }
        }

        private static string GetDataQuran(IRequestWeb requestHandler)
        {
            return requestHandler.GetReleases(APIUrl.BaseUrl);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.RowIndex > 0)
                {
                    richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    link = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    richTextBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PlayMp3FromUrl(string url, int timeout)
        {
            try
            {
                using (Stream ms = new MemoryStream())
                {
                    using (Stream stream = WebRequest.Create(url)
                        .GetResponse().GetResponseStream())
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                    }
                    ms.Position = 0;
                    using (WaveStream blockAlignedStream =
                        new BlockAlignReductionStream(
                            WaveFormatConversionStream.CreatePcmStream(
                                new Mp3FileReader(ms))))
                    {
                        using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                        {
                            waveOut.Init(blockAlignedStream);
                            waveOut.PlaybackStopped += (sender, e) =>
                            {
                                waveOut.Stop();
                            };
                            waveOut.Play();
                            waiting = true;
                            stop.WaitOne(timeout);
                            waiting = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var playThread = new Thread(timeout => PlayMp3FromUrl(link, (int)timeout));
            playThread.IsBackground = true;
            playThread.Start(10800000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (waiting)
            {
                stop.Set();
            } 
        }
    }
}
