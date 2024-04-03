import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Resident, ResidentResponse } from './resident';
import { UserInfo } from './userInfo';

@Injectable({
  providedIn: 'root'
})
export class ResidentService {

  resident!: Resident;

  constructor(private httpClient: HttpClient) { }

  getResident(): Resident {
    if (!this.resident) {
      const route = '/auth/me';
      this.httpClient.get<Resident>(route)
        .subscribe(r => this.resident = r);
    }
    return this.resident;
  }
}
