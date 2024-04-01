import { Component, OnInit } from '@angular/core';
import { PostService } from '../post.service';
import { Post } from '../post';
import { ResidentService } from '../resident.service';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-discussion',
  templateUrl: './discussion.component.html',
  styleUrls: ['./discussion.component.css']
})
export class DiscussionComponent implements OnInit {

  posts!: Post[];

  newPostModel: Post = {
    id: '2',
    author: {
      id: '',
      userName: '',
      picture: '',
      condominium: {
        name: 'f',
        id: ''
      }
    },
    text: 'f',
    createdAt: new Date("2020-01-01"),
  }

  constructor(private postService: PostService, private residentService: ResidentService) { }

  ngOnInit(): void {
    this.postService.getPosts().subscribe(posts => this.posts = posts);
  }

  onSubmit(): void {
    this.newPostModel.author = { ...this.residentService.resident };
    this.newPostModel.id = uuidv4();
    this.newPostModel.createdAt = new Date();
    this.postService.createPost(this.newPostModel).subscribe(
      _ => this.posts.push(this.newPostModel));
  }

}
