import { Injectable } from '@angular/core';
import { CreatePostResponse, Post, PostResponse } from './post';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
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

  constructor(private httpClient: HttpClient) { }

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

  getPosts(endCursor?: string): Observable<Post[]> {
    const variables = {
      endCursor
    };
    const requestBody = {
      query: endCursor ? this.getPostsAfterQuery : this.getPostsQuery,
      variables
    }

    return this.httpClient.post<PostResponse>(
      this.gqlEndpointRelativeUrl, requestBody, this.httpOptions)
      .pipe(map((d) => d.data.posts.items));
  }
}
