import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../api/axios";

export default function RegisterAdmin() {

    const navigate = useNavigate();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function register(e) {

        e.preventDefault();

        try {

            await api.post("/auth/register/admin", {
                username,
                password
            });

            alert("Admin registered successfully.");

            navigate("/");

        }
        catch {

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

                    Admin Registration

                </h2>

                <form onSubmit={register}>

                    <div className="mb-3">

                        <label className="form-label">

                            Username

                        </label>

                        <input
                            className="form-control"
                            value={username}
                            onChange={(e)=>setUsername(e.target.value)}
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
                            onChange={(e)=>setPassword(e.target.value)}
                        />

                    </div>

                    <button
                        className="btn btn-dark w-100"
                        type="submit"
                    >
                        Register Admin
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