import React from 'react';
import { View, Text, StyleSheet, Dimensions } from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faFish, faDroplet, faFaceSmile, faHeart, faBolt } from '@fortawesome/free-solid-svg-icons';

const getGradientColors = (value) => {
  const maxValue = 100;
  const percentage = value / maxValue;

  // Define color stops for the gradient
  const colors = [
    { stop: 0, color: '#b71c1c' },   // Dark red
    { stop: 25, color: '#ff8f00' },  // Orange
    { stop: 50, color: '#ffee58' },  // Yellow
    { stop: 75, color: '#398c3c' },  // Dark green
    { stop: 100, color: '#0bf413' }, // Light green
  ];

  // Find appropriate color stops based on percentage
  let lowerColor, upperColor;
  for (let i = 0; i < colors.length - 1; i++) {
    if (percentage * 100 >= colors[i].stop && percentage * 100 <= colors[i + 1].stop) {
      lowerColor = colors[i];
      upperColor = colors[i + 1];
      break;
    }
  }

  return [lowerColor.color, upperColor.color];
};

library.add(faFish, faDroplet, faFaceSmile, faHeart, faBolt);

const StatsCircleComponent = ({ selectedCat }) => {
  const radius = 30;
  const diameter = radius * 2;

  const stats = ['hunger', 'thirst', 'happiness', 'health', 'energy'];
  const icons = ["fa-fish", "fa-droplet", "fa-face-smile", "fa-heart", "fa-bolt"];

  return (
    <View style={styles.container}>
      {stats.map((stat, index) => {
        const value = selectedCat[stat];
        const gradientColors = getGradientColors(value);

        // Calculate the fill percentage
        const fillPercentage = value / 100;

        return (
          <View key={index} style={styles.statContainer}>
            <View style={[styles.circle, { width: diameter, height: diameter, borderRadius: radius }]}>
              <LinearGradient
                colors={gradientColors}
                start={{ x: 1, y: 1 }}
                end={{ x: 1, y: 0 }}
                style={[ styles.fill, { borderRadius: 0, height: `${fillPercentage * 100}%` }]}
              />
              <FontAwesomeIcon icon={icons[index]} size={22} style={styles.icon} />
            </View>
          </View>
        );
      })}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-around',
    width: '100%',
    padding: 5,
  },
  statContainer: {
    alignItems: 'center',
    margin: 0,
  },
  statTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 5,
  },
  circle: {
    borderWidth: 2,
    borderColor: 'black',
    position: 'relative',
    justifyContent: 'center',
    alignItems: 'center',
    overflow: 'hidden',
  },
  fill: {
    width: '100%',
    bottom: 0,
    position: 'absolute'
  },
  icon: {
    color: 'black',
    position: 'absolute',
    zIndex: 1,
  },
});

export default StatsCircleComponent;
