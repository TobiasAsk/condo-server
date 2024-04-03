import { Inject, Injectable } from '@angular/core';
import { CreatePostResponse, Post, PostResponse } from './post';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map, of } from 'rxjs';
import { CONDOMINIUM_ID } from './user.condominium';

@Injectable({
  providedIn: 'root',
})
export class PostService {

  private condominiumId: string;

  constructor(
    private httpClient: HttpClient,
    @Inject(CONDOMINIUM_ID) condominiumId: string) {
    this.condominiumId = condominiumId;
  }

  createPost(post: Post): Observable<CreatePostResponse> {
    return this.httpClient.post<CreatePostResponse>(
      `/condominiums/${this.condominiumId}/posts`, post);
  }

  getPosts(): Observable<Post[]> {
    return this.httpClient.get<Post[]>(`/condominiums/${this.condominiumId}/posts`);
  }
}
