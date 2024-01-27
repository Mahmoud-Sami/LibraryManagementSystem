using LMS.Business;
using LMS.Business.Commands.Account.Register;
using MediatR;

namespace LMS.UI.Desktop.Forms
{
    public partial class frmRegister : Form
    {
        private readonly IMediator _mediator;
        public frmRegister(IMediator mediator)
        {
            InitializeComponent();
            this._mediator = mediator;
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            Result result = await _mediator.Send(new RegisterCommand(txtName.Text.Trim(), txtUsername.Text.Trim(), txtPassword.Text, txtConfirmPassword.Text));

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                this.Close();
            }
            else
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void ClearFields()
        {
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }
    }
}
