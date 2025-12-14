import {useEffect, useState} from "react";
import { useNavigate } from 'react-router';
import './Authorization.css';
import {ApiHelper} from "./ApiHelper.jsx";

const Authorization = ({
                           onLoginSuccess,
                           title = "Привет!",
                           subtitle = "Войдите в свой аккунт"
                       }) => {
    const [user, setUser] = useState({});
    const [isLogin, setIsLogin] = useState(true);
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: ''
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const apiHelper = new ApiHelper();
    //const { login } = useAuth();
    
    useEffect(() => {
        navigateNext();
    }, [])
    const navigateNext = () => {
        if (localStorage.getItem('marketplace-user-id')) {
            navigate('/main');
        }
    };

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
        // Clear error when user types
        if (error) setError('');
    };

    const validateForm = () => {
        if (!formData.email || !formData.password) {
            setError('Please fill in all required fields');
            return false;
        }

        if (!/\S+@\S+\.\S+/.test(formData.email)) {
            setError('Please enter a valid email');
            return false;
        }

        if (isLogin) return true;

        // Signup validation
        if (formData.password.length < 6) {
            setError('Password must be at least 6 characters');
            return false;
        }

        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match');
            return false;
        }

        return true;
    };

    const handleAuth = async () => {
        try {
            let userCredentials = {
                email: formData.email,
                password: formData.password
            };
            const response = await fetch(`${apiHelper.userManipulationBaseAddress}/Authorize`, {
                method: 'POST',
                headers: {
                    'accept': '*/*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userCredentials),
            });
            if (!response.ok) {
                let localError = await response.json();
                alert(localError.message);
                return;
            }
            const result = await response.json();
            localStorage.setItem('marketplace-user-id', result.id);
            navigateNext();
        } catch (err) {
            setError(err);
        }
    }
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateForm()) return;

        setIsLoading(true);
        setError('');

        try {
            const credentials = {
                email: formData.email,
                password: formData.password,
                ...(isLogin ? {} : {name: formData.email.split('@')[0]})
            };

            await handleAuth();
            //await login(credentials);
        } catch (err) {
            setError(err.message || 'Authentication failed. Please try again.');
        } finally {
            setIsLoading(false);
        }
    };

    if (isLoading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;

    return (
        <>
            <div className="auth-container">
                <div className="auth-card">
                    <div className="auth-header">
                        <h1 className="auth-title">{title}</h1>
                        <p className="auth-subtitle">{subtitle}</p>
                    </div>

                    {error && (
                        <div className="auth-error" role="alert">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className="auth-form">
                        <div className="form-group">
                            <label htmlFor="email">Email Address / Nick</label>
                            <input
                                id="email"
                                name="email"
                                type="email"
                                value={formData.email}
                                onChange={handleChange}
                                className="auth-input"
                                required
                                aria-describedby={error ? "error-message" : undefined}
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="password">Password</label>
                            <input
                                id="password"
                                name="password"
                                type="password"
                                value={formData.password}
                                onChange={handleChange}
                                className="auth-input"
                                required
                            />
                        </div>

                        <button
                            type="submit"
                            className="auth-button"
                            disabled={isLoading}
                            aria-busy={isLoading}
                        >
                            {isLoading ? 'Processing...' : 'Войти'}
                        </button>
                    </form>

                    <div className="auth-toggle">
                        <p>
                            Don't have an account?
                        </p>
                        <button
                            type="button"
                            onClick={() => {
                                setIsLogin(!isLogin);
                                setError('');
                                setFormData({
                                    email: '',
                                    password: '',
                                    confirmPassword: ''
                                });
                            }}
                            className="auth-link"
                        >
                            Sign Up
                        </button>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Authorization;