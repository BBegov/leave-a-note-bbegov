import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { Outlet } from 'react-router-dom';

function App() {
  return (
    <div className="App">
      <header className='appHeader'>
        <Navbar bg="light" expand="lg">
          <Container>
            <Navbar.Brand href="/home">Leave a Note</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="me-auto">
                <Nav.Link href="home">Home of Notes</Nav.Link>
                <Nav.Link href="mynotes">My Notes</Nav.Link>
                <Nav.Link href="users">Users</Nav.Link>
              </Nav>
            </Navbar.Collapse>
            <Navbar.Collapse className="justify-content-end">
              <Navbar.Text>
                Signed in as: <a href="#login">placeholder</a>
              </Navbar.Text>
            </Navbar.Collapse>
          </Container>
        </Navbar>
      </header>
      <content className="content">
        <Outlet />
      </content>
    </div>
  );
}

export default App;
