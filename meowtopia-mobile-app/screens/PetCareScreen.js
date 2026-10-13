import React, { useRef, useState, useEffect } from 'react';
import {
  View,
  Text,
  Image,
  StyleSheet,
  ScrollView,
  Dimensions,
  TouchableOpacity,
  SafeAreaView,
  Platform,
  StatusBar,
  Modal,
  ActivityIndicator
} from 'react-native';
import StatsCircleComponent from '../components/StatsCircleComponent';
import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome';
import { faFish, faDroplet, faFutbol, faSoap, faCat } from '@fortawesome/free-solid-svg-icons';
import { library } from '@fortawesome/fontawesome-svg-core';
import PetCareActionModal from '../modals/PetCareActionModal';
import { Dropdown } from 'react-native-element-dropdown';
import LevelComponent from '../components/LevelComponent';
import { fetchCatsByUser } from '../api/cat';

library.add(faFish, faDroplet, faFutbol, faSoap, faCat);

const POLLING_INTERVAL = 60000; // Poll every 60 seconds

const PetCareScreen = () => {
  const [cats, setCats] = useState([]);
  const [selectedCat, setSelectedCat] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [actionType, setActionType] = useState('');
  const [dropdownValue, setDropdownValue] = useState(null);
  const [dropdownVisible, setDropdownVisible] = useState(false);
  const [loading, setLoading] = useState(true);

  const scrollViewRef = useRef(null);
  const userId = '8a3c8c48-f14e-42db-b8e6-803d8962dd95';

  const loadCats = async () => {
    try {
      const fetchedCats = await fetchCatsByUser(userId);
      setCats(fetchedCats);
      if (fetchedCats.length > 0) {
        setSelectedCat(fetchedCats[0]);
        setDropdownValue(fetchedCats[0]?.catId);
      }
    } catch (error) {
      console.error('Failed to fetch cats', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCats();

    const interval = setInterval(loadCats, POLLING_INTERVAL);

    console.log(cats);

    return () => clearInterval(interval); // Cleanup interval on component unmount
  }, []);

  const handleAction = (action) => {
    setActionType(action);
    setModalVisible(true);
  };

  const performAction = () => {
    // Perform the action (e.g., feeding, watering)
    setModalVisible(false);
  };

  const closeModal = () => {
    setModalVisible(false);
  };

  const onDropdownChange = (item) => {
    const selectedCat = cats.find(cat => cat.catId === item.value);
    setSelectedCat(selectedCat);
    setDropdownValue(item.value);
    const index = cats.findIndex(cat => cat.catId === item.value);
    scrollViewRef.current.scrollTo({ x: index * Dimensions.get('window').width, animated: true });
    setDropdownVisible(false);
  };

  if (loading) {
    return (
      <View style={styles.loaderContainer}>
        <ActivityIndicator size="large" color="#0000ff" />
      </View>
    );
  }

  if (cats.length === 0) {
    return (
      <View style={styles.emptyContainer}>
        <Text>No cats found.</Text>
      </View>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <LevelComponent label="1" />
        <TouchableOpacity onPress={() => setDropdownVisible(true)}>
          <FontAwesomeIcon icon="fa-cat" size={40} color="#000" />
        </TouchableOpacity>
      </View>

      <Modal visible={dropdownVisible} transparent={true} animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Dropdown
              style={styles.dropdown}
              containerStyle={styles.dropdownContainer}
              data={cats.map(cat => ({ label: cat.name, value: cat.catId }))}
              labelField="label"
              valueField="value"
              placeholder="Select a cat"
              value={dropdownValue}
              onChange={onDropdownChange}
            />
            <TouchableOpacity onPress={() => setDropdownVisible(false)} style={styles.closeButton}>
              <Text style={styles.closeButtonText}>Close</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>

      <ScrollView
        horizontal
        pagingEnabled
        showsHorizontalScrollIndicator={false}
        ref={scrollViewRef}
        style={styles.scrollView}
        onScroll={(e) => {
          const index = Math.round(e.nativeEvent.contentOffset.x / Dimensions.get('window').width);
          setSelectedCat(cats[index]);
          setDropdownValue(cats[index].catId);
        }}
        scrollEventThrottle={16}
      >
        {cats.map((cat) => (
          <View key={cat.catId} style={styles.catScreen}>
            <Text style={styles.catDetailsTitle}>{cat.name}</Text>
            <Image source={{ uri: cat.image }} style={styles.catImage} />
            <StatsCircleComponent selectedCat={selectedCat} />
            <Text>{cat.breedName}</Text>
          </View>
        ))}
      </ScrollView>

      <View style={styles.buttonContainer}>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleAction('feed')}>
          <FontAwesomeIcon icon="fa-fish" size={18} color="white" />
        </TouchableOpacity>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleAction('water')}>
          <FontAwesomeIcon icon="fa-droplet" size={18} color="white" />
        </TouchableOpacity>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleAction('play')}>
          <FontAwesomeIcon icon="fa-futbol" size={18} color="white" />
        </TouchableOpacity>
        <TouchableOpacity style={styles.actionButton} onPress={() => handleAction('clean')}>
          <FontAwesomeIcon icon="fa-soap" size={18} color="white" />
        </TouchableOpacity>
      </View>

      <PetCareActionModal
        modalVisible={modalVisible}
        actionType={actionType}
        selectedCat={selectedCat}
        performAction={performAction}
        closeModal={closeModal}
      />
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F5FCFF',
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight : 0,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 10,
    paddingHorizontal: 15,
  },
  dropdown: {
    width: '100%',
    height: 40,
    backgroundColor: '#e0e0e0',
    borderRadius: 12,
    padding: 10,
  },
  dropdownContainer: {
    backgroundColor: '#e0e0e0',
  },
  scrollView: {
    flex: 1,
    margin: 0,
    padding: 0,
  },
  catScreen: {
    width: Dimensions.get('window').width,
    paddingHorizontal: 15,
    alignItems: 'center',
  },
  catDetailsTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 5,
    textTransform: 'uppercase',
  },
  catImage: {
    width: '100%',
    height: '50%',
    borderRadius: 15,
    marginBottom: 10,
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    paddingVertical: 10,
    borderTopWidth: 1,
    borderColor: '#DDD',
    backgroundColor: '#FFF',
  },
  actionButton: {
    backgroundColor: '#4CAF50',
    padding: 10,
    borderRadius: 30,
    alignItems: 'center',
    justifyContent: 'center',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    width: '90%',
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
    alignItems: 'center',
  },
  closeButton: {
    marginTop: 20,
    backgroundColor: '#4CAF50',
    padding: 10,
    borderRadius: 5,
  },
  closeButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  loaderContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
});

export default PetCareScreen;
