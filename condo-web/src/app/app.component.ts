import { HttpClient } from '@angular/common/http';
import { Component, InjectionToken } from '@angular/core';
import { Observable, map } from 'rxjs';
import { UserInfo, MeResponse } from './userInfo';
import { ResidentService } from './resident.service';
import { Resident } from './resident';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'my-app';

  constructor(private httpClient: HttpClient, private residentService: ResidentService) { }

  get userInfo() { return this.residentService.resident }
  ngOnInit(): void {
    this.residentService.getResident();
  }
}
