import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Homepage from '../pages/Homepage';
import LoginPage from '../pages/LoginPage';
import MyNotesPage from '../pages/MyNotesPage';
import Userspage from '../pages/Userspage';

const AppRouter = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" >
                    <Route path="home" element={<Homepage />} />
                    <Route path="mynotes" element={<MyNotesPage />} />
                    <Route path="login" element={<LoginPage />} />
                    <Route path="users" element={<Userspage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
};

export default AppRouter;