import type { Post } from '../types/post';

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const fetchService = {
  // 1. GET: Fetch list of posts
  async getPosts(): Promise<Post[]> {
    const response = await fetch(`${BASE_URL}/posts?_limit=10`);
    if (!response.ok) {
      throw new Error(`Fetch GET posts failed: ${response.status} ${response.statusText}`);
    }
    return await response.json();
  },

  // 2. GET: Fetch single post by ID
  async getPostById(id: number | string): Promise<Post> {
    const response = await fetch(`${BASE_URL}/posts/${id}`);
    if (!response.ok) {
      throw new Error(`Fetch GET post #${id} failed: ${response.status} ${response.statusText}`);
    }
    return await response.json();
  },

  // 3. POST: Create a new post
  async createPost(newPost: Omit<Post, 'id'>): Promise<Post> {
    const response = await fetch(`${BASE_URL}/posts`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: JSON.stringify(newPost),
    });
    if (!response.ok) {
      throw new Error(`Fetch POST failed: ${response.status} ${response.statusText}`);
    }
    return await response.json();
  },

  // 4. PUT: Update post by ID
  async updatePost(id: number | string, updatedPost: Partial<Post>): Promise<Post> {
    const response = await fetch(`${BASE_URL}/posts/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: JSON.stringify(updatedPost),
    });
    if (!response.ok) {
      throw new Error(`Fetch PUT failed: ${response.status} ${response.statusText}`);
    }
    return await response.json();
  },

  // 5. DELETE: Delete post by ID
  async deletePost(id: number | string): Promise<boolean> {
    const response = await fetch(`${BASE_URL}/posts/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) {
      throw new Error(`Fetch DELETE failed: ${response.status} ${response.statusText}`);
    }
    return true;
  },
};
