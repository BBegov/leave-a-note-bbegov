import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Notes from './components/Notes';
import Users from './components/Users';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />}>
          <Route path="home" element={<Notes url={"https://localhost:44321/api/Notes"} />} />
          <Route path="mynotes" element={<Notes url={"https://localhost:44321/api/Users/1/Notes"} />} />
          <Route path="users" element={<Users url={"https://localhost:44321/api/Users"} />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
