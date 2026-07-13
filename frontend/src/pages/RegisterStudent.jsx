import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function RegisterStudent() {

    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function register(e) {

        e.preventDefault();

        try {

            await api.post("/auth/register/student", {
                name,
                email,
                username,
                password
            });

            alert("Student registered successfully.");

            navigate("/");

        }
        catch (error) {

            console.log(error.response?.data);

            alert("Registration failed.");

        }

    }

    return (

        <div
            className="container d-flex justify-content-center align-items-center"
            style={{ minHeight: "100vh" }}
        >

            <div
                className="card shadow p-4"
                style={{ width: "450px" }}
            >

                <h2 className="text-center mb-4">
                    Student Registration
                </h2>

                <form onSubmit={register}>

                    <div className="mb-3">

                        <label className="form-label">
                            Name
                        </label>

                        <input
                            className="form-control"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />

                    </div>

                    <div className="mb-3">

                        <label className="form-label">
                            Email
                        </label>

                        <input
                            className="form-control"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />

                    </div>

                    <div className="mb-3">

                        <label className="form-label">
                            Username
                        </label>

                        <input
                            className="form-control"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />

                    </div>

                    <div className="mb-3">

                        <label className="form-label">
                            Password
                        </label>

                        <input
                            className="form-control"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />

                    </div>

                    <button
                        className="btn btn-success w-100"
                        type="submit"
                    >
                        Register Student
                    </button>

                </form>

                <Link
                    className="btn btn-link mt-3"
                    to="/"
                >
                    Back to Login
                </Link>

            </div>

        </div>

    );

}