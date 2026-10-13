import * as React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faHouse } from '@fortawesome/free-solid-svg-icons/faHouse'
import { faPaw } from '@fortawesome/free-solid-svg-icons/faPaw'
import { faBoxOpen } from '@fortawesome/free-solid-svg-icons/faBoxOpen';
import { faCartShopping } from '@fortawesome/free-solid-svg-icons/faCartShopping';
import { faCog } from '@fortawesome/free-solid-svg-icons/faCog';
import HomeScreen from './screens/HomeScreen';
import PetCareScreen from './screens/PetCareScreen.js';
import InventoryScreen from './screens/InventoryScreen.js';
import ShopScreen from './screens/ShopScreen.js';
import SettingsScreen from './screens/ProfileScreen.js';

const Tab = createBottomTabNavigator();

library.add(faHouse, faPaw, faBoxOpen, faCartShopping, faCog);

function App() {
  return (
    <NavigationContainer>
      <Tab.Navigator>
        <Tab.Screen
          name="Home"
          component={HomeScreen}
          options={{
            headerShown: false,
            tabBarIcon: ({ color, size }) => (
              <FontAwesomeIcon icon="fa-house" color={color} size={size} />
            ),
          }}
        />
        <Tab.Screen
          name="Pet Care"
          component={PetCareScreen}
          options={{
            headerShown: false,
            tabBarIcon: ({ color, size }) => (
              <FontAwesomeIcon icon="fa-paw" color={color} size={size} />            ),
          }}
        />
        <Tab.Screen
          name="Inventory"
          component={InventoryScreen}
          options={{
            headerShown: false,
            tabBarIcon: ({ color, size }) => (
              <FontAwesomeIcon icon="fa-box-open" color={color} size={size} />
            ),
          }}
        />
        <Tab.Screen
          name="Shop"
          component={ShopScreen}
          options={{
            headerShown: false,
            tabBarIcon: ({ color, size }) => (
              <FontAwesomeIcon icon="fa-cart-shopping" color={color} size={size} />
            ),
          }}
        />
        <Tab.Screen
          name="Settings"
          component={SettingsScreen}
          options={{
            headerShown: false,
            tabBarIcon: ({ color, size }) => (
              <FontAwesomeIcon icon="fa-cog" color={color} size={size} />
            ),
          }}
        />
      </Tab.Navigator>
    </NavigationContainer>
  );
}

export default App;
