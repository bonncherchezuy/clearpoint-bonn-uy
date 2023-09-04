import { createContext } from "react";


export const ApiContext = createContext({
  uri: `${process.env.REACT_APP_BASEURI || ''}/api/todoitems`
});

