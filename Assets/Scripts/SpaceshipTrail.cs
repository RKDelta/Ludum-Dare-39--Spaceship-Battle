using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipTrail : MonoBehaviour
{
    public GameObject trailParticlePrefab;

    public int bufferSize = 100;

    struct ParticleTime
    {
        public float timeStart;
        public GameObject particle;
    }

    Queue<GameObject> inactiveParticles;
    Queue<ParticleTime> activeParticles;

    public float minDist = 0.1f;

    public float maxTime = 1f;

    public GameObject spaceship;

    Vector3 lastPos;

    void Start()
    {
        this.inactiveParticles = new Queue<GameObject>();
        this.activeParticles = new Queue<ParticleTime>();

        for (int i = 0; i < this.bufferSize; i++)
        {
            GameObject go = GameObject.Instantiate(trailParticlePrefab, this.transform);

            go.SetActive(false);

            this.inactiveParticles.Enqueue(go);
        }
    }

    void Update()
    {
        float dist = (this.spaceship.transform.position - this.lastPos).magnitude;

        while (dist > this.minDist)
        {
            GameObject go = this.inactiveParticles.Dequeue();

            go.transform.position = Vector3.Lerp(this.lastPos, this.spaceship.transform.position, this.minDist/dist);
            go.transform.rotation = this.spaceship.transform.rotation;

            go.SetActive(true);

            this.lastPos = go.transform.position;

            this.activeParticles.Enqueue(new ParticleTime() { timeStart = Time.time, particle = go });

            dist -= this.minDist;
        }
        
        while (this.activeParticles.Count > 0 && (Time.time - this.activeParticles.Peek().timeStart) >= maxTime)
        {
            ParticleTime activeParticle = this.activeParticles.Dequeue();

            activeParticle.particle.SetActive(false);

            this.inactiveParticles.Enqueue(activeParticle.particle);
        }
    }
}
