using LMS.Business;
using LMS.Business.Commands.Account.Login;
using LMS.Business.DTOs;
using LMS.DataAccess.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.UI.Desktop.Forms
{
    public partial class frmMain : Form
    {
        private readonly IMediator _mediator;
        private readonly frmRegister _frmRegister;
        private readonly frmBooks _frmBooks;
        public frmMain(frmRegister frmRegister, frmBooks frmBooks, IMediator mediator)
        {
            InitializeComponent();
            _frmRegister = frmRegister;
            _frmBooks = frmBooks;
            _mediator = mediator;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            _frmRegister.ShowDialog(this);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            Result<UserDTO> result = await _mediator.Send(new LoginCommand(txtUsername.Text.Trim(), txtPassword.Text));
            if (result.Success)
            {
                ClearFields();
                _frmBooks.currentUser = result.Data;
                _frmBooks.Show();
                this.Hide();
            }
            else
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
    }
}
