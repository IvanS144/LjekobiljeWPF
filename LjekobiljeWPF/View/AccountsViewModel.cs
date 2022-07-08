﻿using Ljekobilje.Dialogs;
using Ljekobilje.Projections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljekobilje.View
{
    public class AccountsViewModel
    {
        private ObservableCollection<CooperantAccountProjection> _accounts = new ObservableCollection<CooperantAccountProjection>();

        public ObservableCollection<CooperantAccountProjection> Accounts { get { return _accounts; } }

        public string Bank { get; set; }

        public string AccountNumber { get; set; }

        private int _cid;

        public DelegateCommand<CooperantAccountProjection> DeleteCommand { get; set; }

        public DelegateCommand<CooperantAccountProjection> UpdateCommand { get; set; }

        public ActionCommand AddCommand { get; set; }

        public AccountsViewModel(int CooperantId)
        {
            _cid = CooperantId;
            using(LjekobiljeEntities entities = new LjekobiljeEntities())
            {
                (from account in entities.CooperantBankAccounts.Include("Account") where account.AccountId == CooperantId select account).ToList().ForEach(a => _accounts.Add(new CooperantAccountProjection(a)));
            }
            DeleteCommand = new DelegateCommand<CooperantAccountProjection>(Delete);
            UpdateCommand = new DelegateCommand<CooperantAccountProjection>(Update);
            AddCommand = new ActionCommand(AddAccount);
        }

        public void Update(CooperantAccountProjection account)
        {
            UpdateCooperantAccountDialog dialog = new UpdateCooperantAccountDialog(account);
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                try
                {
                    (account.Bank, account.AccountNumber) = dialog.getFields();
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(account.Account.Account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        entities.SaveChanges();
                    }
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Ažuriranje nije uspjelo, pokušajte ponovo" : "Failed to update bank account, try again");
                }
            }
        }

        public void AddAccount()
        {
            try
            {
                using (LjekobiljeEntities entities = new LjekobiljeEntities())
                {
                    BankAccount account = new BankAccount();
                    account.Bank = Bank;
                    account.AccountNumber = AccountNumber;
                    account = entities.BankAccounts.Add(account).Entity;
                    entities.SaveChanges();
                    CooperantBankAccount cooperantAccount = new CooperantBankAccount();
                    cooperantAccount.CooperantId = _cid;
                    cooperantAccount.AccountId = account.AccountId;
                    cooperantAccount = entities.CooperantBankAccounts.Add(cooperantAccount).Entity;
                    entities.SaveChanges();
                    _accounts.Add(new CooperantAccountProjection(cooperantAccount));
                }
            }
            catch
            {

                new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Kreiranje nije uspjelo, provjerite polja koja ste unjeli i pokušajte ponovo" : "Failed to add new bank account, check the fileds you entered and try again");
            }
        }

        public void Delete(CooperantAccountProjection account)
        {
            YesNoDialog dialog = new YesNoDialog();
            bool? result = dialog.ShowDialog((App.GetLanguage() == 1 ? "Da li zaista želite da obrišete račun čiji je id " : "Do you want to delete account whose id is ") + account.AccountId + " ?");
            if(result == true)
            {
                try
                {
                    using (LjekobiljeEntities entities = new LjekobiljeEntities())
                    {
                        entities.Entry(account.Account).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        entities.Entry(account.Account.Account).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        entities.SaveChanges();
                    }
                    Accounts.Remove(account);
                }
                catch
                {

                    new ErrorDialog().ShowDialog(App.GetLanguage() == 1 ? "Nije moguće obisati ovaj račun" : "It is not possible to delete this bank account");
                }
            }
        }
    }
}
