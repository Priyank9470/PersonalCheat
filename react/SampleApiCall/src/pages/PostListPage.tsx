import { useState, useEffect } from 'react';
import { Link, useOutletContext } from 'react-router-dom';
import { fetchService } from '../api/fetchService';
import { axiosService } from '../api/axiosService';
import type { Post } from '../types/post';
import { slugify } from '../utils/slugify';

interface LayoutContext {
  apiMode: 'fetch' | 'axios';
}

export const PostListPage = () => {
  const { apiMode } = useOutletContext<LayoutContext>();
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  const notify = (msg: string) => {
    setStatusMessage(msg);
    setTimeout(() => setStatusMessage(null), 4000);
  };

  // Select service dynamically based on Navbar engine toggle
  const getService = () => (apiMode === 'fetch' ? fetchService : axiosService);

  const loadPosts = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getService().getPosts();
      setPosts(data);
      notify(`GET Success: Loaded ${data.length} posts using ${apiMode.toUpperCase()} engine.`);
    } catch (err: any) {
      setError(err.message || 'Failed to load posts');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPosts();
  }, [apiMode]);

  const handleDelete = async (id: number) => {
    if (!window.confirm(`Are you sure you want to delete post #${id}?`)) return;

    setLoading(true);
    try {
      await getService().deletePost(id);
      setPosts((prev) => prev.filter((p) => p.id !== id));
      notify(`DELETE Success: Post #${id} deleted via ${apiMode.toUpperCase()}.`);
    } catch (err: any) {
      setError(err.message || 'Failed to delete post');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <div>
          <h2>Post Listing (Default Screen)</h2>
          <p className="page-subtitle">
            Current Engine: <strong>{apiMode.toUpperCase()}</strong> (Base URL from <code>.env</code>)
          </p>
        </div>
        <Link to="/posts/new" className="btn primary">
          + Create New Post
        </Link>
      </div>

      {statusMessage && <div className="banner success">{statusMessage}</div>}
      {error && <div className="banner error">{error}</div>}

      {loading && posts.length === 0 ? (
        <p className="loading-text">Loading post listing via {apiMode.toUpperCase()}...</p>
      ) : (
        <div className="post-grid">
          {posts.map((post) => {
            const titleSlug = slugify(post.title);
            return (
              <div key={post.id} className="post-card">
                <div className="post-card-header">
                  <span className="badge">ID: #{post.id}</span>
                  <span className="slug-preview">/{titleSlug}</span>
                </div>
                <h3>{post.title}</h3>
                <p>{post.body.substring(0, 100)}...</p>

                <div className="post-card-actions">
                  {/* Dynamic URL Route using Post Title Slug */}
                  <Link
                    to={`/posts/${post.id}/${titleSlug}`}
                    className="btn info-sm"
                  >
                    View Details (Dynamic Title URL)
                  </Link>

                  <Link to={`/posts/edit/${post.id}`} className="btn warning-sm">
                    Edit (PUT)
                  </Link>

                  <button
                    onClick={() => post.id && handleDelete(post.id)}
                    className="btn danger-sm"
                    disabled={loading}
                  >
                    Delete
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};
