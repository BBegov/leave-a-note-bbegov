import { Outlet } from 'react-router-dom';

const Content = () => {
    return (
        <main className="content">
            <Outlet />
        </main>
    );
};

export default Content;