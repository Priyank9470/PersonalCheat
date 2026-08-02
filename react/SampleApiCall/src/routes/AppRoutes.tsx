import { Routes, Route, Navigate } from 'react-router-dom';
import { Layout } from '../components/Layout';
import { PostListPage } from '../pages/PostListPage';
import { PostDetailPage } from '../pages/PostDetailPage';
import { PostFormPage } from '../pages/PostFormPage';
import { PostFormikPage } from '../pages/PostFormikPage';

export const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        {/* Default screen: Listing Page */}
        <Route index element={<PostListPage />} />
        <Route path="posts" element={<PostListPage />} />

        {/* Add & Edit Post with Custom Form Validation (No Packages) */}
        <Route path="posts/new" element={<PostFormPage />} />
        <Route path="posts/edit/:id" element={<PostFormPage />} />

        {/* Add & Edit Post with Formik + Yup */}
        <Route path="posts/formik/new" element={<PostFormikPage />} />
        <Route path="posts/formik/edit/:id" element={<PostFormikPage />} />

        {/* Post Details Screen with Dynamic Title URL */}
        <Route path="posts/:id/:titleSlug" element={<PostDetailPage />} />

        {/* Catch-all redirect to listing screen */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  );
};
