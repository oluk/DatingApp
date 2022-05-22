import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { User } from '../_models/User';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
 baseURL = "https://localhost:5001/api/";
 private _currentusersource = new ReplaySubject<User>(1);
 currentUser$ = this._currentusersource.asObservable();
  constructor(private httpclient: HttpClient) {
      
   }

   login(model: any)
   {

     return this.httpclient.post(this.baseURL +'account/login', {un: model.username, pw:model.password} ).pipe(
       map( (response: User ) => {
          const user = response;
          if(user){
            localStorage.setItem('user', JSON.stringify(user));
            this._currentusersource.next(user);
          }
       }

       )
     )
   }

   setCurrentUser(user: User)
   {
     this._currentusersource.next(user);
   }

   logout()
   {
     localStorage.removeItem('user');
     this._currentusersource.next(null);
   }
}
