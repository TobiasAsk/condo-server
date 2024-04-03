import { Resident } from "./resident";
import { UserInfo } from "./userInfo";

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
}
