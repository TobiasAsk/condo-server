import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Resident, ResidentResponse } from './resident';

@Injectable({
  providedIn: 'root'
})
export class ResidentService {

  resident!: Resident;

  private gqlEndpointRelativeUrl = '/data-api/graphql';

  private getResidentQuery = `query Resident_by_pk($residentId: ID) {
    resident_by_pk(id: $residentId) {
        id
        name
        profilePictureUrl
        condominium {
            name
            id
        }
    }
  }`;

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient: HttpClient) { }

  getResident(residentId: string): Resident {
    if (!this.resident) {
      const variables = {
        residentId
      };
      const requestBody = {
        query: this.getResidentQuery,
        variables
      }

      this.httpClient.post<ResidentResponse>(
        this.gqlEndpointRelativeUrl, requestBody, this.httpOptions)
        .pipe(map((d) => d.data.resident_by_pk))
        .subscribe(r => this.resident = r);
    }
    return this.resident;
  }
}
