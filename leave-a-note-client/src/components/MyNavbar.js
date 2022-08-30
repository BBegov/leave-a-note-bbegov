import Container from 'react-bootstrap/Container';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';

const MyNavbar = () => {
    return (
        <div className='navbarContainer'>
            <Navbar bg="primary" variant='dark' expand="lg">
                <Container fluid>
                    <Navbar.Brand href="/home">Leave a Note</Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll" />
                    <Navbar.Collapse id="navbarScroll">
                        <Nav className="me-auto">
                            <Nav.Link href="home">Home of Notes</Nav.Link>
                            <Nav.Link href="mynotes">My Notes</Nav.Link>
                            <Nav.Link href="users">Users</Nav.Link>
                        </Nav>
                        <Navbar.Text>
                            Signed in as: <a href="#login">placeholder</a>
                        </Navbar.Text>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </div>
    );
};

export default MyNavbar;