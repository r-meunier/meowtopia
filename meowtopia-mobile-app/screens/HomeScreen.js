import React, { useEffect, useState } from 'react';
import { View, Text, FlatList, Image, StyleSheet, SafeAreaView, Platform, StatusBar } from 'react-native';
import { fetchCatsByUser } from '../api/cat';

function HomeScreen() {
  return (
    <SafeAreaView style={styles.container}>
    {/* Stats container */}
      <Image source={require('../assets/MaineCoon6-Tortoise.png')} style={styles.catImage} />

      <Text>Home Screen Content</Text>
      
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    backgroundColor: '#F5FCFF', // blue #F5FCFF green #D9FADA
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight : 0,
  },
    catImage: {
        width: 300,
        height: 300,
        marginBottom: 20,
      },
});

export default HomeScreen;
