using System.Windows.Forms;
using System.Net;
using System;

namespace ChatServidor
{
    public partial class Form1 : Form
    {
        private delegate void AtualizaStatusCallback(string strMensagem);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAtender_Click(object sender, System.EventArgs e)
        {
            if (txtIP.Text==string.Empty)
            {
                MessageBox.Show("Informe o endereço IP.");
                txtIP.Focus();
                return;
            }
            
            //Verifica se a porta digitada está em formato válido, se não estiver, coloca no valor padrão 21010.
            if (int.TryParse(txtPort.Text, out int boolporta) == false)
            {
                MessageBox.Show("A porta informada está com valor inválido. O valor padrão (21010) será atribuido.");
                txtPort.Text = "21010";
            }
            if (int.Parse(txtPort.Text) > 65335 || int.Parse(txtPort.Text) <= 0)
            {
                MessageBox.Show("A porta selecionada está fora dos alcance de portas TCP. O programa utilizará o valor padrão (21010).");
                txtPort.Text = "21010";
            }
            try
            {
                // Analisa o endereço IP do servidor informado no textbox
                IPAddress enderecoIP = IPAddress.Parse(txtIP.Text);
                int porta = int.Parse(txtPort.Text);
                // Cria uma nova instância do objeto ChatServidor
                ChatServidor mainServidor = new ChatServidor(enderecoIP, porta);

                // Vincula o tratamento de evento StatusChanged a mainServer_StatusChanged
                ChatServidor.StatusChanged += new StatusChangedEventHandler(mainServidor_StatusChanged);



                
                // Inicia o atendimento das conexões
                mainServidor.IniciaAtendimento();
                txtLog.AppendText("Monitorando as conexões na porta " + porta + " ...\r\n");
                txtPort.Enabled = false;
                txtIP.Enabled = false;
                btnAtender.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message);
            }
            
        }

        public void mainServidor_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // Chama o método que atualiza o formulário
            this.Invoke(new AtualizaStatusCallback(this.AtualizaStatus), new object[] { e.EventMessage });
        }

        private void AtualizaStatus(string strMensagem)
        {
            // Atualiza o logo com mensagens
            txtLog.AppendText(strMensagem + "\r\n");
        }
    }
}
