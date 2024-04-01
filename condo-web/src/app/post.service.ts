import { Inject, Injectable } from '@angular/core';
import { CreatePostResponse, Post, PostResponse } from './post';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map, of } from 'rxjs';
import { CONDOMINIUM_ID } from './user.condominium';

@Injectable({
  providedIn: 'root',
})
export class PostService {

  private gqlEndpointRelativeUrl = '/data-api/graphql';
  private getPostsAfterQuery = `query GetPosts($endCursor: String) {
    posts(first: 3, after: $endCursor) {
        items {
            id
            text
            author {
              id
              name
              profilePictureUrl
            }
        }
        hasNextPage
        endCursor
    }
  }`;
  private getPostsQuery = `query GetPosts {
    posts(first: 10) {
        items {
            id
            text
            author {
              id
              name
              profilePictureUrl
              condominium {
                id
                name
              }
            }
            createdAt
        }
        hasNextPage
        endCursor
    }
  }`;

  private createPostMutation = `mutation CreatePost($post: CreatePostInput!) {
    createPost(item: $post) {
        id
    }
  }`;

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private condominiumId: string;

  constructor(
    private httpClient: HttpClient,
    @Inject(CONDOMINIUM_ID) condominiumId: string) {
    this.condominiumId = condominiumId;
  }

  createPost(post: Post): Observable<CreatePostResponse> {
    const variables = {
      post
    };
    const requestBody = {
      query: this.createPostMutation,
      variables
    }

    return this.httpClient.post<CreatePostResponse>(
      this.gqlEndpointRelativeUrl, requestBody, this.httpOptions);
  }

  getPosts(): Observable<Post[]> {
    return this.httpClient.get<Post[]>(`/condominiums/${this.condominiumId}/posts`);
  }
}
