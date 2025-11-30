import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Offcanvas from 'react-bootstrap/Offcanvas';
import LoggedLogo from './assets/LoggedLogo.jsx';
import NotLoggedLogo from './assets/NotLoggedLogo.jsx';
import {Dropdown} from "react-bootstrap";
import {useState, useEffect} from "react";
import { useNavigate } from 'react-router';

function Header() {
    const expand = 'md';
    const isLoggedIn = localStorage.getItem('marketplace-user-id') !== null;
    const [user, setUser] = useState({});
    const navigate = useNavigate();
    const [screenSize, setScreenSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });
    
    useEffect(() => {
        const handleResize = () => {
            setScreenSize({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        };

        window.addEventListener('resize', handleResize);
        getUserData();

        // Cleanup the event listener when the component unmounts
        return () => {
            window.removeEventListener('resize', handleResize);
        };
    },[]);
    const logOutHandler = () => {
        localStorage.removeItem('marketplace-user-id');
        navigate('/');
    };
    const walletReplenishment = async () => {
        try {
            let money = 300;
            let query = `https://localhost:7004/UserManipulations/WalletReplenishment?userId=${user.id}&money=${money}`;
            const response = await fetch(query, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }//,
                //body: JSON.stringify(requestBody),
            });
    
            if (!response.ok) {
                //throw new Error(`HTTP error! status: ${response.status}`);
                alert(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            setUser(data);
        } catch (error) {
            console.error('Error updating user:', error);
        }
    };
    const getUserData = async () => {
        try {
            let id = localStorage.getItem('marketplace-user-id');
            let query = `https://localhost:7004/UserManipulations/${id}`;
            const response = await fetch(query, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }//,
                //body: JSON.stringify(requestBody),
            });

            if (!response.ok) {
                //throw new Error(`HTTP error! status: ${response.status}`);
                alert(`HTTP error! status: ${response.status}`);
                localStorage.removeItem('marketplace-user-id');
                navigate('/');
            }

            const data = await response.json();
            setUser(data);
        } catch (error) {
            console.error('Error updating user:', error);
        }
    }

    return (
        <>
            <Navbar key={expand} expand={expand} className="bg-body-tertiary mb-3"
                    style={{borderRadius: '21px', backgroundColor: '#ececec'}}>
                <Container fluid>
                    <Navbar.Brand href="/">Marketplace</Navbar.Brand>
                    <Navbar.Toggle aria-controls={`offcanvasNavbar-expand-${expand}`}/>
                    <Navbar.Offcanvas
                        id={`offcanvasNavbar-expand-${expand}`}
                        aria-labelledby={`offcanvasNavbarLabel-expand-${expand}`}
                        placement="end"
                    >
                        <Offcanvas.Header closeButton>
                            <Offcanvas.Title id={`offcanvasNavbarLabel-expand-${expand}`}>
                                Marketplace
                            </Offcanvas.Title>
                        </Offcanvas.Header>
                        <Offcanvas.Body>
                            <Nav className="justify-content-start flex-grow-1 pe-3">
                                <Nav.Link href="/main">Home</Nav.Link>
                                <Nav.Link href="/main/purchases">History</Nav.Link>
                            </Nav>
                            <NavDropdown
                                drop={screenSize.width < 768 ? 'down-centered' : 'start'}
                                title={isLoggedIn ? <LoggedLogo/> : <NotLoggedLogo/>}
                                id={`offcanvasNavbarDropdown-expand-${expand}`}
                            >
                                {isLoggedIn && <NavDropdown.Item>{user.name}</NavDropdown.Item>}
                                {isLoggedIn && <NavDropdown.Item>Баланс: {user.wallet}</NavDropdown.Item>}
                                {
                                    isLoggedIn &&
                                    <NavDropdown.Item onClick={walletReplenishment}>
                                        Пополнить баланс
                                    </NavDropdown.Item>
                                }
                                {isLoggedIn && <NavDropdown.Divider/>}
                                {
                                    isLoggedIn &&
                                    <NavDropdown.Item onClick={logOutHandler}>
                                        Выход
                                    </NavDropdown.Item>
                                }

                            </NavDropdown>
                        </Offcanvas.Body>
                    </Navbar.Offcanvas>
                </Container>
            </Navbar>
        </>
    );
}

export default Header;