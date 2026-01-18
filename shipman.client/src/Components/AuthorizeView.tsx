import { useState, useEffect, createContext } from "react";
import { Navigate } from "react-router";

const UserContext = createContext({});

interface User {
    name: string;
}
export default function AuthorizeView(props: { children: React.ReactNode }) {
    const [authorized, setAuthorized] = useState(false);
    const [user, setUser] = useState<User>({ name: "" });
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function fetchAuth() {
            try {
                const response = await fetch("getUserName", { method: "GET" });

                // Only parse JSON if the server actually returned JSON
                const contentType = response.headers.get("content-type") || "";

                if (response.ok && contentType.includes("application/json")) {
                    const u = await response.json();
                    setUser({ name: u.name });
                    setAuthorized(true);
                } else {
                    setAuthorized(true);
                }
            } catch (err) {
                setAuthorized(false);
            } finally {
                setLoading(false);
            }
        }

        fetchAuth();
    }, []);

    // Still loading ? show loading screen
    if (loading) {
        return <p>Loading...</p>;
    }

    // Not authorized ? redirect to login
    if (!authorized) {
        return <Navigate to="/login" />;
    }

    // Authorized ? render children properly
    return (
        <UserContext.Provider value={user}>
            {props.children}
        </UserContext.Provider>
    );
}






{/*import { useState, useEffect, createContext } from "react";
import { Navigate } from "react-router";
const UserContext = createContext({});
interface User {
    name: string;
}
export default function AuthorizeView(props: { children: React.ReactNode }) {

    const [authorized, setAuthorized] = useState<boolean>(false);
    const [user, setUser] = useState<User>({ name: "" });
    const [loading, setLoading] = useState<boolean>(true);
    useEffect(() => {

        async function fetchAuth(url: string, options: RequestInit) {
            const response = await fetch(url, options);
            if (response.status == 200) {
                const u = await response.json();
                setUser({ name: u.name });
                setAuthorized(true);
                return response;
            }
            else {
                return response;
            }
        }

        try {
            fetchAuth('getUserName', { method: "GET", })
        } catch (error: unknown) {

            if (error instanceof Error) {
                console.log(error.message);
            }
        }
        finally { setLoading(false); }
    }, []);
}
    if (loading) {
        return (
            <>
                <p>Loading...</p>
            </>
        )
    }
    else {
        if (authorized && !loading) {
            return (
                <>
                    <UserContext.Provider value={user}></UserContext.Provider>
                </>
            );  
        } else {
            return (
                <>
                    <Navigate to ="/login"/>
                </>
            );
        }
    }
}
*/}