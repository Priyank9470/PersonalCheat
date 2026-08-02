import axiosInstance from './axiosInstance';
import type { Post } from '../types/post';

export const axiosService = {
  // 1. GET: Fetch list of posts
  async getPosts(): Promise<Post[]> {
    const response = await axiosInstance.get<Post[]>('/posts?_limit=10');
    return response.data;
  },

  // 2. GET: Fetch single post by ID
  async getPostById(id: number | string): Promise<Post> {
    const response = await axiosInstance.get<Post>(`/posts/${id}`);
    return response.data;
  },

  // 3. POST: Create a new post
  async createPost(newPost: Omit<Post, 'id'>): Promise<Post> {
    const response = await axiosInstance.post<Post>('/posts', newPost);
    return response.data;
  },

  // 4. PUT: Update post by ID
  async updatePost(id: number | string, updatedPost: Partial<Post>): Promise<Post> {
    const response = await axiosInstance.put<Post>(`/posts/${id}`, updatedPost);
    return response.data;
  },

  // 5. DELETE: Delete post by ID
  async deletePost(id: number | string): Promise<boolean> {
    await axiosInstance.delete(`/posts/${id}`);
    return true;
  },
};
