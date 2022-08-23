import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css';
import App from './App';
import Home from './components/Home';
import Users from './components/Users';

const BaseUrl = "https://localhost:44321/api";
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />}>
          <Route path="home" element={<Home url={BaseUrl + "/Notes"} />} />
          <Route path="mynotes" element={<Home url={BaseUrl + "/Users/2/Notes"} />} />
          <Route path="users" element={<Users url={BaseUrl + "/Users"} />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
