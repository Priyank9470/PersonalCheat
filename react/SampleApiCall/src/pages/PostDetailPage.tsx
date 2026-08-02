import { useState, useEffect } from 'react';
import { useParams, Link, useOutletContext } from 'react-router-dom';
import { fetchService } from '../api/fetchService';
import { axiosService } from '../api/axiosService';
import type { Post } from '../types/post';

interface LayoutContext {
  apiMode: 'fetch' | 'axios';
}

export const PostDetailPage = () => {
  const { id, titleSlug } = useParams<{ id: string; titleSlug: string }>();
  const { apiMode } = useOutletContext<LayoutContext>();

  const [post, setPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const getService = () => (apiMode === 'fetch' ? fetchService : axiosService);

  useEffect(() => {
    if (!id) return;
    const fetchDetail = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getService().getPostById(id);
        setPost(data);
      } catch (err: any) {
        setError(err.message || 'Failed to fetch post details.');
      } finally {
        setLoading(false);
      }
    };
    fetchDetail();
  }, [id, apiMode]);

  return (
    <div className="page-container">
      <div className="page-header">
        <Link to="/" className="btn secondary">
          ← Back to Listing
        </Link>
        <span className="badge">Dynamic URL Route</span>
      </div>

      <div className="url-banner">
        <span>Current Dynamic URL Path: </span>
        <code>/posts/{id}/{titleSlug}</code>
      </div>

      {loading ? (
        <p className="loading-text">Loading post details via {apiMode.toUpperCase()}...</p>
      ) : error ? (
        <div className="banner error">{error}</div>
      ) : post ? (
        <div className="detail-card">
          <div className="detail-header">
            <h2>{post.title}</h2>
            <span className="badge primary">Post ID: #{post.id}</span>
          </div>

          <div className="detail-body">
            <h4>Post Description / Content:</h4>
            <p>{post.body}</p>
          </div>

          <div className="detail-meta">
            <p><strong>Author User ID:</strong> #{post.userId || 1}</p>
            <p><strong>Title Slug (URL parameter):</strong> <code>{titleSlug}</code></p>
            <p><strong>API Engine Used:</strong> {apiMode.toUpperCase()}</p>
          </div>

          <div className="detail-actions">
            <Link to={`/posts/edit/${post.id}`} className="btn warning">
              Edit Post (PUT)
            </Link>
          </div>
        </div>
      ) : null}
    </div>
  );
};
