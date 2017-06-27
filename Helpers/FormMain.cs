using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Helpers
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private String toScaledString(double value, int digits)
        {
            String smag = "f";
            double dmag = 0.000000000000001;

            if (value >= 0.000000000001) { smag = " p"; dmag = 0.000000000001; }
            if (value >= 0.000000001) { smag = " n"; dmag = 0.000000001; }
            if (value >= 0.000001) { smag = " µ"; dmag = 0.000001; }
            if (value >= 0.001) { smag = " m"; dmag = 0.001; }
            if (value >= 1) { smag = " "; dmag = 1; }
            if (value >= 1000.0) { smag = " k"; dmag = 1000.0; }
            if (value >= 1000000.0) { smag = " M"; dmag = 1000000.0; }
            if (value >= 1000000000.0) { smag = " G"; dmag = 1000000000.0; }
            if (value >= 1000000000000.0) { smag = " T"; dmag = 1000000000000.0; }
            if (value >= 1000000000000000.0) { smag = " P"; dmag = 1000000000000000.0; }

            return (value / dmag).ToString("N" + digits) + smag;
        }

        #region DC RC Circuit Page

        private int DCRC_Selected_Box = 4;
        private double DCRCInitial = 0;
        private double DCRCDesired = 0;
        private double DCRCSupply = 0;
        private double DCRCTime = 0;
        private double DCRCTimeScalar = 0.001;
        private double DCRCR = 0;
        private double DCRCRScalar = 1000;
        private double DCRCC = 0;
        private double DCRCCScalar = 0.000001;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 1;
            textDCRCInitial.Enabled = false;
            textDCRCDesired.Enabled = true;
            textDCRCSupply.Enabled = true;
            textDCRCTime.Enabled = true;
            textDCRCR.Enabled = true;
            textDCRCC.Enabled = true;
            pictureBox1.Image = Properties.Resources.DCRCInitial;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 2;
            textDCRCInitial.Enabled = true;
            textDCRCDesired.Enabled = false;
            textDCRCSupply.Enabled = true;
            textDCRCTime.Enabled = true;
            textDCRCR.Enabled = true;
            textDCRCC.Enabled = true;
            pictureBox1.Image = Properties.Resources.DCRCDesired;
        }

        private void radDCRCSupply_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 3;
            textDCRCInitial.Enabled = true;
            textDCRCDesired.Enabled = true;
            textDCRCSupply.Enabled = false;
            textDCRCTime.Enabled = true;
            textDCRCR.Enabled = true;
            textDCRCC.Enabled = true;
            pictureBox1.Image = Properties.Resources.DCRCSupply;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 4;
            textDCRCInitial.Enabled = true;
            textDCRCDesired.Enabled = true;
            textDCRCSupply.Enabled = true;
            textDCRCTime.Enabled = false;
            textDCRCR.Enabled = true;
            textDCRCC.Enabled = true;
            pictureBox1.Image = Properties.Resources.DCRCTime;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 5;
            textDCRCInitial.Enabled = true;
            textDCRCDesired.Enabled = true;
            textDCRCSupply.Enabled = true;
            textDCRCTime.Enabled = true;
            textDCRCR.Enabled = false;
            textDCRCC.Enabled = true;
            pictureBox1.Image = Properties.Resources.DCRCR;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            DCRC_Selected_Box = 6;
            textDCRCInitial.Enabled = true;
            textDCRCDesired.Enabled = true;
            textDCRCSupply.Enabled = true;
            textDCRCTime.Enabled = true;
            textDCRCR.Enabled = true;
            textDCRCC.Enabled = false;
            pictureBox1.Image = Properties.Resources.DCRCC;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (DCRC_Selected_Box)
            {
                case 1: // Solve for Initial
                    textDCRCInitial.Text = (DCRCSupply + (DCRCDesired - DCRCSupply) * Math.Exp(DCRCTime / (DCRCR * DCRCC))).ToString();
                    break;

                case 2: // Solve for Desired
                    textDCRCDesired.Text = (DCRCSupply + (DCRCInitial - DCRCSupply) * Math.Exp(-DCRCTime / (DCRCR * DCRCC))).ToString();
                    break;

                case 3: // Solve for Supply
                    textDCRCSupply.Text = ((DCRCDesired * Math.Exp(DCRCTime / (DCRCR * DCRCC)) - DCRCInitial) / (Math.Exp(DCRCTime / (DCRCR * DCRCC)) - 1)).ToString();
                    break;

                case 4: // Solve for Time
                    textDCRCTime.Text = (DCRCR * DCRCC * Math.Log((DCRCInitial - DCRCSupply) / (DCRCDesired - DCRCSupply)) / DCRCTimeScalar).ToString();
                    break;

                case 5: // Solve for R
                    textDCRCR.Text = (DCRCTime / (DCRCC * Math.Log((DCRCInitial - DCRCSupply) / (DCRCDesired - DCRCSupply))) / DCRCRScalar).ToString();
                    break;

                case 6: // Solve for C
                    textDCRCC.Text = (DCRCTime / (DCRCR * Math.Log((DCRCInitial - DCRCSupply) / (DCRCDesired - DCRCSupply))) / DCRCCScalar).ToString();
                    break;

                default:// Do Nothing
                    break;
            }
        }

        private void textDCRCInitial_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCInitial = Convert.ToDouble(textDCRCInitial.Text);
            }
            catch (FormatException)
            {
                textDCRCInitial.Text = "0";
            }
        }

        private void textDCRCDesired_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCDesired = Convert.ToDouble(textDCRCDesired.Text);
            }
            catch (FormatException)
            {
                textDCRCDesired.Text = "0";
            }
        }

        private void textDCRCSupply_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCSupply = Convert.ToDouble(textDCRCSupply.Text);
            }
            catch (FormatException)
            {
                textDCRCSupply.Text = "0";
            }
        }

        private void textDCRCTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCTime = Convert.ToDouble(textDCRCTime.Text) * DCRCTimeScalar;
            }
            catch (FormatException)
            {
                textDCRCTime.Text = "0";
            }
        }

        private void textDCRCR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCR = Convert.ToDouble(textDCRCR.Text) * DCRCRScalar;
            }
            catch (FormatException)
            {
                textDCRCR.Text = "0";
            }
        }

        private void textDCRCC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCRCC = Convert.ToDouble(textDCRCC.Text) * DCRCCScalar;
            }
            catch (FormatException)
            {
                textDCRCC.Text = "0";
            }
        }

        private void selectDCRCTimeUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectDCRCTimeUnits.SelectedIndex)
            {
                case 0: // hour
                    DCRCTimeScalar = 3600.0;
                    break;

                case 1: // minute
                    DCRCTimeScalar = 60.0;
                    break;

                case 2: //second
                    DCRCTimeScalar = 1.0;
                    break;

                case 3: //millisecond
                    DCRCTimeScalar = 1.0 / 1000.0;
                    break;

                case 4: //microsecond
                    DCRCTimeScalar = 1.0 / 1000000.0;
                    break;

                default:
                    break;
            }
        }

        private void selectDCRCRUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectDCRCRUnits.SelectedIndex)
            {
                case 0: // ohm
                    DCRCRScalar = 1.0;
                    break;

                case 1: // kiloohm
                    DCRCRScalar = 1000.0;
                    break;

                case 2: //megaohm
                    DCRCRScalar = 1000000.0;
                    break;

                default:
                    break;
            }
        }

        private void selectDCRCCUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (selectDCRCCUnits.SelectedIndex)
            {
                case 0: //picoFarad
                    DCRCCScalar = 1.0 / 1000000000000.0;
                    break;

                case 1: //nanoFarad
                    DCRCCScalar = 1.0 / 1000000000.0;
                    break;

                case 2: //microFarad
                    DCRCCScalar = 1.0 / 1000000.0;
                    break;

                case 3: // milliFarad
                    DCRCCScalar = 1.0 / 1000.0;
                    break;

                case 4: // Farad
                    DCRCCScalar = 1.0;
                    break;

                default:
                    break;
            }
        }

        private void buttonDCRCResGo_Click(object sender, EventArgs e)
        {
            textSPCVDesired.Text = textDCRCR.Text;
            boxSPCVMagnitude.SelectedIndex = selectDCRCRUnits.SelectedIndex + 5;
            tabControl2.SelectedTab = tabSpecs;
            tabControl3.SelectedTab = tabSPCV;
        }

        private void buttonDCRCCapGo_Click(object sender, EventArgs e)
        {
            textSPCVDesired.Text = textDCRCC.Text;
            boxSPCVMagnitude.SelectedIndex = selectDCRCCUnits.SelectedIndex + 1;
            tabControl2.SelectedTab = tabSpecs;
            tabControl3.SelectedTab = tabSPCV;
        }

        #endregion DC RC Circuit Page

        #region DC Voltage Divider Page

        private int DCVD_Selected_Box = 2;
        private double DCVD_VIN = 0;
        private double DCVD_VOUT = 0;
        private double DCVD_RUP = 0;
        private double DCVD_RDOWN = 0;
        private double DCVD_RUP_Scalar = 1000;
        private double DCVD_RDOWN_Scalar = 1000;

        private void radDCVD_Vin_CheckedChanged(object sender, EventArgs e)
        {
            DCVD_Selected_Box = 1;
            textDCVD_Vin.Enabled = false;
            textDCVD_Vout.Enabled = true;
            textDCVD_Rup.Enabled = true;
            textDCVD_Rdown.Enabled = true;
        }

        private void radDCVD_Vout_CheckedChanged(object sender, EventArgs e)
        {
            DCVD_Selected_Box = 2;
            textDCVD_Vin.Enabled = true;
            textDCVD_Vout.Enabled = false;
            textDCVD_Rup.Enabled = true;
            textDCVD_Rdown.Enabled = true;
        }

        private void radDCVD_Rup_CheckedChanged(object sender, EventArgs e)
        {
            DCVD_Selected_Box = 3;
            textDCVD_Vin.Enabled = true;
            textDCVD_Vout.Enabled = true;
            textDCVD_Rup.Enabled = false;
            textDCVD_Rdown.Enabled = true;
        }

        private void radDCVD_Rdown_CheckedChanged(object sender, EventArgs e)
        {
            DCVD_Selected_Box = 4;
            textDCVD_Vin.Enabled = true;
            textDCVD_Vout.Enabled = true;
            textDCVD_Rup.Enabled = true;
            textDCVD_Rdown.Enabled = false;
        }

        private void textDCVD_Vin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCVD_VIN = Convert.ToDouble(textDCVD_Vin.Text);
            }
            catch (FormatException)
            {
                textDCVD_Vin.Text = "0";
            }
        }

        private void textDCVD_Vout_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCVD_VOUT = Convert.ToDouble(textDCVD_Vout.Text);
            }
            catch (FormatException)
            {
                textDCVD_Vout.Text = "0";
            }
        }

        private void textDCVD_Rup_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCVD_RUP = Convert.ToDouble(textDCVD_Rup.Text);
            }
            catch (FormatException)
            {
                textDCVD_Rup.Text = "0";
            }
        }

        private void textDCVD_Rdown_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DCVD_RDOWN = Convert.ToDouble(textDCVD_Rdown.Text);
            }
            catch (FormatException)
            {
                textDCVD_Rdown.Text = "0";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: // ohm
                    DCVD_RUP_Scalar = 1.0;
                    break;

                case 1: // kiloohm
                    DCVD_RUP_Scalar = 1000.0;
                    break;

                case 2: //megaohm
                    DCVD_RUP_Scalar = 1000000.0;
                    break;

                default:
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0: // ohm
                    DCVD_RDOWN_Scalar = 1.0;
                    break;

                case 1: // kiloohm
                    DCVD_RDOWN_Scalar = 1000.0;
                    break;

                case 2: //megaohm
                    DCVD_RDOWN_Scalar = 1000000.0;
                    break;

                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (DCVD_Selected_Box)
            {
                case 1:
                    textDCVD_Vin.Text = (DCVD_VOUT * (1.0 + ((DCVD_RUP * DCVD_RUP_Scalar) / (DCVD_RDOWN * DCVD_RDOWN_Scalar)))).ToString();
                    break;

                case 2:
                    textDCVD_Vout.Text = (DCVD_VIN / (1.0 + ((DCVD_RUP * DCVD_RUP_Scalar) / (DCVD_RDOWN * DCVD_RDOWN_Scalar)))).ToString();
                    break;

                case 3:
                    textDCVD_Rup.Text = (((DCVD_RDOWN * DCVD_RDOWN_Scalar) / ((DCVD_VIN / DCVD_VOUT) - 1)) / DCVD_RUP_Scalar).ToString();
                    break;

                case 4:
                    textDCVD_Rdown.Text = (((DCVD_RUP * DCVD_RUP_Scalar) * ((DCVD_VIN / DCVD_VOUT) - 1)) / DCVD_RDOWN_Scalar).ToString();
                    break;

                default:
                    break;
            }
            labelDCVD_RES.Text = toScaledString((DCVD_RDOWN * DCVD_RDOWN_Scalar) + (DCVD_RUP * DCVD_RUP_Scalar), 3) + "Ω";
            labelDCVD_CUR.Text = toScaledString(DCVD_VIN / ((DCVD_RDOWN * DCVD_RDOWN_Scalar) + (DCVD_RUP * DCVD_RUP_Scalar)), 3) + "A";
            labelDCVD_POW.Text = toScaledString(DCVD_VIN * DCVD_VIN / ((DCVD_RDOWN * DCVD_RDOWN_Scalar) + (DCVD_RUP * DCVD_RUP_Scalar)), 3) + "W";
        }

        #endregion DC Voltage Divider Page

        #region Standard Passive Component Values

        private Color[] RESISTOR_COLOR = { Color.Black, Color.Brown, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Violet, Color.Gray, Color.White, Color.Gold, Color.Silver };
        private int[] IEC_E192 = { 100, 101, 102, 104, 105, 106, 107, 109, 110, 111, 113, 114, 115, 117, 118, 120, 121, 123, 124, 126, 127, 129, 130, 132, 133, 135, 137, 138, 140, 142, 143, 145, 147, 149, 150, 152, 154, 156, 158, 160, 162, 164, 165, 167, 169, 172, 174, 176, 178, 180, 182, 184, 187, 189, 191, 193, 196, 198, 200, 203, 205, 208, 210, 213, 215, 218, 221, 223, 226, 229, 232, 234, 237, 240, 243, 246, 249, 252, 255, 258, 261, 264, 267, 271, 274, 277, 280, 284, 287, 291, 294, 298, 301, 305, 309, 312, 316, 320, 324, 328, 332, 336, 340, 344, 348, 352, 357, 361, 365, 370, 378, 379, 383, 388, 392, 397, 402, 407, 412, 417, 422, 427, 432, 437, 442, 448, 453, 459, 464, 470, 475, 481, 487, 493, 499, 505, 511, 517, 523, 530, 536, 542, 549, 556, 562, 569, 576, 583, 590, 597, 604, 612, 619, 626, 634, 642, 649, 657, 665, 673, 681, 690, 698, 706, 715, 723, 732, 741, 750, 759, 768, 777, 787, 796, 806, 816, 825, 835, 845, 856, 866, 876, 887, 898, 909, 920, 931, 942, 953, 965, 976, 988 };
        private int[] IEC_E96 = { 100, 102, 105, 107, 110, 113, 115, 118, 121, 124, 127, 130, 133, 137, 140, 143, 147, 150, 154, 158, 162, 165, 169, 174, 178, 182, 187, 191, 196, 200, 210, 215, 221, 226, 232, 237, 243, 249, 255, 261, 267, 274, 280, 287, 294, 301, 309, 316, 324, 332, 340, 348, 357, 365, 374, 383, 392, 402, 412, 422, 432, 442, 453, 464, 475, 487, 499, 511, 523, 536, 549, 562, 576, 590, 604, 619, 634, 649, 665, 681, 698, 715, 732, 750, 768, 787, 806, 825, 845, 866, 887, 909, 931, 953, 976 };
        private int[] IEC_E48 = { 100, 105, 110, 115, 121, 127, 133, 140, 147, 154, 162, 169, 178, 187, 196, 205, 215, 226, 237, 249, 261, 274, 287, 301, 316, 332, 348, 365, 383, 402, 422, 442, 464, 487, 511, 536, 562, 590, 619, 649, 681, 715, 750, 787, 825, 866, 909, 953 };
        private int[] IEC_E24 = { 100, 110, 120, 130, 150, 160, 180, 200, 220, 240, 270, 300, 330, 360, 390, 430, 470, 510, 560, 620, 680, 750, 820, 910 };
        private int[] IEC_E12 = { 100, 120, 150, 180, 220, 270, 330, 390, 470, 560, 680, 820 };
        private int[] IEC_E6 = { 100, 150, 220, 330, 470, 680 };
        private double SPCV_INPUT_SCALAR = 1;
        private double DesiredVal;
        private int ActualVal;
        private int Magnitude = 0;
        private string MagChar = "";
        private bool isFiveBand = false;

        private void SPCVCalculate(object sender, EventArgs e)
        {
            try
            { DesiredVal = Convert.ToDouble(textSPCVDesired.Text); }
            catch (FormatException)
            { textSPCVDesired.Text = "0"; return; }

            DesiredVal *= SPCV_INPUT_SCALAR;
            if (DesiredVal == 0) { return; }
            int[] IEC_VALS;
            if (radioSPCVT05.Checked) { IEC_VALS = IEC_E192; panelR5.BackColor = RESISTOR_COLOR[5]; isFiveBand = true; }
            else if (radioSPCVT1.Checked) { IEC_VALS = IEC_E96; panelR5.BackColor = RESISTOR_COLOR[1]; isFiveBand = true; }
            else if (radioSPCVT2.Checked) { IEC_VALS = IEC_E48; panelR5.BackColor = RESISTOR_COLOR[2]; isFiveBand = true; }
            else if (radioSPCVT5.Checked) { IEC_VALS = IEC_E24; panelR5.BackColor = RESISTOR_COLOR[10]; isFiveBand = false; }
            else if (radioSPCVT10.Checked) { IEC_VALS = IEC_E12; panelR5.BackColor = RESISTOR_COLOR[11]; isFiveBand = false; }
            else if (radioSPCVT20.Checked) { IEC_VALS = IEC_E6; panelR5.BackColor = Color.Transparent; ; isFiveBand = false; } //None
            else { IEC_VALS = IEC_E24; }

            // Here we scale down the desired value until we get a number within the IEC value range.
            // Keeping track of the order of magnitude we have to use.
            Magnitude = 0;
            while (DesiredVal < IEC_VALS[0])
            {
                DesiredVal = DesiredVal * 10;
                Magnitude--;
            }
            while (DesiredVal > IEC_VALS[IEC_VALS.Count() - 1])
            {
                DesiredVal = DesiredVal / 10;
                Magnitude++;
            }

            int i = 0;
            int underflow = 0;
            double[] between;
            while (i < IEC_VALS.Count())
            {
                if (IEC_VALS[i] > DesiredVal) { break; }
                i++;
            }; //This takes us to our actual magnitude color band.

            if (i > 0)
            {
                between = new double[] { IEC_VALS[i - 1], IEC_VALS[i] };
            }
            else
            {
                between = new double[] { IEC_VALS[IEC_VALS.Count() - 1] * Math.Pow(10, -1), IEC_VALS[i] };
                underflow = 1;
            }

            if (radioSPCVClosest.Checked)
            {
                if (Math.Abs(DesiredVal - between[0]) < Math.Abs(DesiredVal - between[1]))
                {
                    ActualVal = Convert.ToInt16(between[0]);
                }
                else
                {
                    ActualVal = Convert.ToInt16(between[1]);
                    underflow = 0;
                }
            }
            else if (radioSPCVAbove.Checked)
            {
                ActualVal = Convert.ToInt16(between[1]);
                underflow = 0;
            }
            else
            {
                ActualVal = Convert.ToInt16(between[0]);
            }

            switch ((Magnitude + 2 - underflow) / 3)
            {
                case -5: MagChar = "f"; break;
                case -4: MagChar = "p"; break;
                case -3: MagChar = "n"; break;
                case -2: MagChar = "µ"; break;
                case 0: MagChar = ""; break;
                case -1: MagChar = "m"; break;
                case 1: MagChar = "k"; break;
                case 2: MagChar = "M"; break;
                case 3: MagChar = "G"; break;
                default: MagChar = "-"; break;
            }

            labelSPCVActual.Text = (ActualVal * Math.Pow(10, ((Magnitude + 2 - underflow) % 3) - 2 + underflow)).ToString() + MagChar;
            labelSPCVError.Text = (((ActualVal - DesiredVal) / DesiredVal) * 100).ToString("0.##") + "%";

            try
            {
                if (isFiveBand)
                {
                    panelR1.BackColor = RESISTOR_COLOR[ActualVal / 100];
                    panelR2.BackColor = RESISTOR_COLOR[(ActualVal / 10) % 10];
                    panelR3.BackColor = RESISTOR_COLOR[ActualVal % 10];

                    if (Magnitude >= 0)
                    {
                        panelR4.BackColor = RESISTOR_COLOR[Magnitude];
                    }
                    else
                    {
                        panelR4.BackColor = RESISTOR_COLOR[12 + Magnitude];
                    }
                }
                else
                {
                    panelR1.BackColor = Color.Transparent;
                    panelR2.BackColor = RESISTOR_COLOR[ActualVal / 100];
                    panelR3.BackColor = RESISTOR_COLOR[(ActualVal / 10) % 10];

                    if (Magnitude >= -1)
                    {
                        panelR4.BackColor = RESISTOR_COLOR[Magnitude + 1];
                    }
                    else
                    {
                        panelR4.BackColor = RESISTOR_COLOR[13 + Magnitude];
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void boxSPCVMagnitude_SelectedIndexChanged(object sender, EventArgs e)
        {
            SPCV_INPUT_SCALAR = 1E-15 * Math.Pow(10, (3 * boxSPCVMagnitude.SelectedIndex));
            SPCVCalculate(sender, e);
        }

        #endregion Standard Passive Component Values
    }
}