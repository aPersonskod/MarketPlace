import {useState} from "react";
import './Authorization.css'
const Registration = ({
                          onLoginSuccess,
                          showSignup = true,
                          title = "Welcome Back",
                          subtitle = "Sign in to your account"
                      }) => {
    const [isLogin, setIsLogin] = useState(true);
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: ''
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    //const { login } = useAuth();

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

            //await login(credentials);

            if (onLoginSuccess) {
                onLoginSuccess();
            }
        } catch (err) {
            setError(err.message || 'Authentication failed. Please try again.');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <>
            <div className="auth-container">
                <div className="auth-card">
                    <div className="auth-header">
                        <h1 className="auth-title">{isLogin ? title : 'Create Account'}</h1>
                        <p className="auth-subtitle">
                            {isLogin ? subtitle : 'Join us today'}
                        </p>
                    </div>

                    {error && (
                        <div className="auth-error" role="alert">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className="auth-form">
                        {!isLogin && (
                            <div className="form-group">
                                <label htmlFor="name">Full Name</label>
                                <input
                                    id="name"
                                    name="name"
                                    type="text"
                                    value={formData.email.split('@')[0] || ''}
                                    disabled
                                    className="auth-input"
                                />
                            </div>
                        )}

                        <div className="form-group">
                            <label htmlFor="email">Email Address</label>
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

                        {!isLogin && (
                            <div className="form-group">
                                <label htmlFor="confirmPassword">Confirm Password</label>
                                <input
                                    id="confirmPassword"
                                    name="confirmPassword"
                                    type="password"
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    className="auth-input"
                                    required
                                />
                            </div>
                        )}

                        <button
                            type="submit"
                            className="auth-button"
                            disabled={isLoading}
                            aria-busy={isLoading}
                        >
                            {isLoading ? 'Processing...' : (isLogin ? 'Sign In' : 'Sign Up')}
                        </button>
                    </form>

                    {showSignup && (
                        <div className="auth-toggle">
                            <p>
                                {isLogin ? "Don't have an account?" : "Already have an account?"}
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
                                {isLogin ? 'Sign Up' : 'Sign In'}
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </>
    );
}

export default Registration;