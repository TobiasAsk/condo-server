import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Observable, map } from 'rxjs';
import { UserInfo, MeResponse } from './userInfo';
import { ResidentService } from './resident.service';
import { Resident } from './resident';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ResidentService]
})
export class AppComponent {
  title = 'my-app';
  userInfo!: UserInfo;

  constructor(private httpClient: HttpClient, private residentService: ResidentService) { }

  ngOnInit(): void {
    this.getUserInfo().subscribe(r => this.userInfo = r);
  }
  
  getUserInfo(): Observable<UserInfo> {
    const route = '/auth/me';
    return this.httpClient.get<UserInfo>(route);
  }
}
