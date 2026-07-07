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

import {getUser, walletReplenishmentRequest} from "./ApiHelper/userService.jsx"
import {ApiHelper} from "./ApiHelper.jsx";

function Header() {
    const expand = 'md';
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [user, setUser] = useState({});
    const navigate = useNavigate();
    const [screenSize, setScreenSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });
    const apiHelper = new ApiHelper();
    
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
        localStorage.removeItem('uToken');
        navigate('/auth');
    };
    const walletReplenishment = async () => {
        try {
            let userDto = await getUser();
            if(userDto === null) navigate('/auth');
            const response = await walletReplenishmentRequest();
            setUser(response);
        } catch (error) {
            console.error('Error updating user:', error);
        }
    };
    const getUserData = async () => {
        try {
            let userDto = await getUser();
            if (userDto !== null){
                setIsLoggedIn(true);
                setUser(userDto);
            }
            //else{ navigate('auth'); }
        } catch (error) {
        
            console.error('Error updating user:', error);
        }
    }

    return (
        <>
            <Navbar key={expand} expand={expand} className="bg-body-tertiary mb-3"
                    style={{borderRadius: '21px', backgroundColor: '#ececec'}}>
                <Container fluid>
                    <Navbar.Brand href={isLoggedIn ? '/' : '/auth'}>Marketplace</Navbar.Brand>
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
                                <Nav.Link href="/">Home</Nav.Link>
                                <Nav.Link href="/purchases">History</Nav.Link>
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
                                {!isLoggedIn && <Button onClick={() => {navigate('/auth');}}>Войти</Button>}
                            </NavDropdown>
                        </Offcanvas.Body>
                    </Navbar.Offcanvas>
                </Container>
            </Navbar>
        </>
    );
}

export default Header;