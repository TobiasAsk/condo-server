import { Resident } from "./resident";
import { UserInfo } from "./userInfo";

export interface Post {
    id: string;
    author: UserInfo;
    text: string;
    createdAt: Date;
}

export interface PostResponse {
    data: { posts: { items: Post[] } };
}

export interface CreatePostResponse {
    data: { createPost: Post };
}
