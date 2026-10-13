import React from 'react';
import {
    View,
    Text,
    StyleSheet,
  } from 'react-native';

function LevelComponent({ label }) {
    return (
        <View style={styles.container}>
            <Text style={styles.label}>{label}</Text>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        backgroundColor: '#e0e0e0',
        width: 40,
        height: 40,
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: 50,
        borderWidth: 2,
        borderColor: 'green',
    },
    label: {
        fontSize: 20,
        fontWeight: 'bold',
        textAlign: 'center',
        margin: 0,
    },
});

export default LevelComponent;