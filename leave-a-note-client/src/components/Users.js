import React from 'react';
import Table from 'react-bootstrap/Table';
import useAxiosFetch from '../hooks/useAxiosFetch';

const Users = ({ url }) => {
    const { data: users, isLoading, fetchError } = useAxiosFetch(url);

    return (
        <main className="userTable">
            {isLoading && <p className="statusMsg">Loading notes...</p>}
            {!isLoading && fetchError && <p className="statusMsg" style={{ color: "red" }}>{fetchError}</p>}
            {!isLoading && !fetchError && (users.length ? (
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>User Id</th>
                            <th>Username</th>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Role</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => {
                            return (
                                <tr key={index}>
                                    <td>{user.id}</td>
                                    <td>{user.userName}</td>
                                    <td>{user.firstName}</td>
                                    <td>{user.lastName}</td>
                                    <td>{user.role}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                </Table>
            ) : <p className="statusMsg">No notes to display.</p>)}
        </main>
    );
};

export default Users;
