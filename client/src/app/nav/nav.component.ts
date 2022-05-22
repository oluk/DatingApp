import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

   model : any = {};
   user : User;

   loggedIn: boolean = false;
  constructor(private accountservice: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  login()
  {
    this.accountservice.login(this.model).subscribe({
      next: response => { this.loggedIn = true; console.log(response)},
      error: error => {console.log(error)}
    })
  }

  logout()
  {
    this.accountservice.logout();
    this.loggedIn = false;
  }

  getCurrentUser()
  {
    this.accountservice.currentUser$.subscribe(
      user => {
        this.loggedIn = !!user;
        if(this.loggedIn)
           this.user = user;
           
      },
      error => {
        console.log(error);
      }
    );
  }

}
