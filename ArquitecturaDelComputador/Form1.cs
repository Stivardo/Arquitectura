﻿using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace ArquitecturaDelComputador
{
    public partial class Form1 : Form
    {
        SerialPort ArduinoUno;
        string passwordBuffer = ""; // Buffer para almacenar la contraseña ingresada
        int intentosFallidos = 0;

        public Form1()
        {
            InitializeComponent();
            ArduinoUno = new SerialPort();

            ArduinoUno.PortName = "COM6";
            ArduinoUno.BaudRate = 9600;
            ArduinoUno.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            try
            {
                ArduinoUno.Open();
                progressBar.Maximum = 100;  // Establece el valor máximo del ProgressBar
                progressBar.Value = 0;      // Inicializa el ProgressBar en 0
                MessageBox.Show("Puerto COM abierto correctamente.");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acceso denegado al puerto COM. Ejecuta como administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el puerto COM: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento para recibir datos del Arduino
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = ArduinoUno.ReadLine(); // Leer los datos recibidos

            this.Invoke(new Action(() =>
            {
                // Mostrar los datos recibidos para depuración
                MessageBox.Show($"Datos recibidos: {data}");

                // Verificar si la contraseña fue incorrecta
                if (data.Contains("Contraseña incorrecta"))
                {
                    intentosFallidos++;
                    progressBar.Value = Math.Min(intentosFallidos * 25, 100);  // Incrementa el ProgressBar en pasos de 25
                    MessageBox.Show($"Contraseña incorrecta. Intentos fallidos: {intentosFallidos}");
                }
                // Verificar si la contraseña fue correcta
                else if (data.Contains("Contraseña correcta"))
                {
                    intentosFallidos = 0;  // Reiniciar el contador de intentos
                    progressBar.Value = 0;  // Reiniciar el ProgressBar
                    MessageBox.Show("Contraseña correcta. Alarma desactivada.");
                }
            }));
        }

        // Envía la contraseña al Arduino una vez que se ingresen los 4 dígitos
        private void EnviarDatosSiCompleto()
        {
            if (passwordBuffer.Length == 4)
            {
                if (ArduinoUno.IsOpen)
                {
                    ArduinoUno.Write(passwordBuffer);  // Envía la contraseña completa al Arduino
                    MessageBox.Show($"Contraseña enviada: {passwordBuffer}");  // Mostrar mensaje de depuración
                }
                else
                {
                    MessageBox.Show("El puerto COM no está abierto.");
                }

                textBoxPassword.Text = "";  // Limpia el campo de la contraseña visible
                passwordBuffer = "";  // Limpia el buffer de la contraseña
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ArduinoUno.IsOpen)
            {
                ArduinoUno.Close();
                MessageBox.Show("Puerto COM cerrado correctamente.");
            }
        }

        // Eventos para botones de números y letras
        private void BotonPresionado(string valor)
        {
            if (passwordBuffer.Length < 4)  // Limitar a 4 dígitos
            {
                passwordBuffer += valor;    // Agrega el valor presionado al buffer
                textBoxPassword.Text += "*"; // Muestra asteriscos en el campo de texto
                EnviarDatosSiCompleto();     // Revisa si ya se completó la contraseña
            }
        }

        private void btn1_Click(object sender, EventArgs e) { BotonPresionado("1"); }
        private void btn2_Click(object sender, EventArgs e) { BotonPresionado("2"); }
        private void btn3_Click(object sender, EventArgs e) { BotonPresionado("3"); }
        private void btnA_Click(object sender, EventArgs e) { BotonPresionado("A"); }
        private void btn4_Click(object sender, EventArgs e) { BotonPresionado("4"); }
        private void btn5_Click(object sender, EventArgs e) { BotonPresionado("5"); }
        private void btn6_Click(object sender, EventArgs e) { BotonPresionado("6"); }
        private void btn7_Click(object sender, EventArgs e) { BotonPresionado("7"); }
        private void btn8_Click(object sender, EventArgs e) { BotonPresionado("8"); }
        private void btn9_Click(object sender, EventArgs e) { BotonPresionado("9"); }
        private void btn0_Click(object sender, EventArgs e) { BotonPresionado("0"); }
        private void btn_michi_Click(object sender, EventArgs e) { BotonPresionado("*"); }
        private void btn_numeral_Click(object sender, EventArgs e) { BotonPresionado("#"); }
        private void btnC_Click(object sender, EventArgs e) { BotonPresionado("C"); }
        private void btnD_Click(object sender, EventArgs e) { BotonPresionado("D"); }
        private void btnB_Click_1(object sender, EventArgs e) { BotonPresionado("B"); }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Opcional: código que quieras ejecutar al cargar el formulario
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            // Opcional: código que quieras ejecutar al interactuar con el ProgressBar
        }
    }
}