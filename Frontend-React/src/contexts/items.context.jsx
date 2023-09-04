import { createContext, useState } from "react";
import axios from 'axios'

export const ItemsContext = createContext({
  items: [],
  setItems: () => null,
  reloadItems: () => null,

  // inputDescription: null,
  // setInputDescription: () => null
});

export const ItemsProvider = ({children}) => 
{
  const [items, setItems] = useState([]);
  const reloadItems = async (apiUri) => 
  {
    try {
      await axios.get(apiUri)
      .then((response) => {setItems(response.data)})
      } catch (error) {
      console.error(error)
    }
  };
  const value = {items, setItems, reloadItems};
  return <ItemsContext.Provider value={value}>{children}</ItemsContext.Provider>
}

