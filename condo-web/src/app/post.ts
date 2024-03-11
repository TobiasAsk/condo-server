import { Resident } from "./resident";

export interface Post {
    id: string;
    author: Resident;
    text: string;
    createdAt: Date;
}

export interface PostResponse {
    data: { posts: { items: Post[] } };
}

export interface CreatePostResponse {
    data: { createPost: Post };
}
