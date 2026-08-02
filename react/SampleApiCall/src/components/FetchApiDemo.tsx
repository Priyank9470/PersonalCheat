import { useState, useEffect } from 'react';
import { fetchService } from '../api/fetchService';
import type { Post } from '../types/post';

export const FetchApiDemo = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  // Helper to show temporary feedback message
  const notify = (msg: string) => {
    setStatusMessage(msg);
    setTimeout(() => setStatusMessage(null), 4000);
  };

  // 1. GET API Call
  const loadPosts = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchService.getPosts();
      setPosts(data);
      notify('GET: Loaded 5 posts via Fetch API');
    } catch (err: any) {
      setError(err.message || 'Failed to load posts');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPosts();
  }, []);

  // 2. PUT API Call
  const handleUpdate = async (id: number) => {
    setLoading(true);
    try {
      const updatedData = {
        title: `Updated Title (Fetch API) at ${new Date().toLocaleTimeString()}`,
        body: 'Updated post body via PUT request.',
      };

      const result = await fetchService.updatePost(id, updatedData);
      
      // Update local state to reflect changes
      setPosts((prevPosts) =>
        prevPosts.map((post) => (post.id === id ? { ...post, ...result } : post))
      );
      
      notify(`PUT Success: Post #${id} updated via Fetch API!`);
    } catch (err: any) {
      setError(err.message || 'Failed to update post');
    } finally {
      setLoading(false);
    }
  };

  // 3. DELETE API Call
  const handleDelete = async (id: number) => {
    setLoading(true);
    try {
      await fetchService.deletePost(id);
      
      // Remove deleted post from local state
      setPosts((prevPosts) => prevPosts.filter((post) => post.id !== id));
      
      notify(`DELETE Success: Post #${id} deleted via Fetch API!`);
    } catch (err: any) {
      setError(err.message || 'Failed to delete post');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="demo-card">
      <h2>Native Fetch API Operations</h2>
      <p className="subtitle">Base URL loaded from <code>.env</code></p>

      {statusMessage && <div className="banner success">{statusMessage}</div>}
      {error && <div className="banner error">{error}</div>}

      <div className="actions">
        <button onClick={loadPosts} disabled={loading} className="btn primary">
          {loading ? 'Fetching...' : 'Reload Posts (GET)'}
        </button>
      </div>

      {loading && posts.length === 0 ? (
        <p className="loading-text">Loading posts via Fetch...</p>
      ) : (
        <ul className="post-list">
          {posts.map((post) => (
            <li key={post.id} className="post-item">
              <div className="post-content">
                <span className="badge">ID: #{post.id}</span>
                <h4>{post.title}</h4>
                <p>{post.body}</p>
              </div>
              <div className="post-actions">
                <button
                  onClick={() => post.id && handleUpdate(post.id)}
                  disabled={loading}
                  className="btn warning"
                >
                  PUT (Update)
                </button>
                <button
                  onClick={() => post.id && handleDelete(post.id)}
                  disabled={loading}
                  className="btn danger"
                >
                  DELETE
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};
