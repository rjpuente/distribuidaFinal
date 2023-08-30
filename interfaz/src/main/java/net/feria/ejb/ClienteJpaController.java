/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.entity.FacturasCabecera;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import net.feria.ejb.exceptions.IllegalOrphanException;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.entity.Cliente;

/**
 *
 * @author Deth1
 */
public class ClienteJpaController implements Serializable {

    public ClienteJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public void create(Cliente cliente) {
        if (cliente.getFacturasCabeceraCollection() == null) {
            cliente.setFacturasCabeceraCollection(new ArrayList<FacturasCabecera>());
        }
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            Collection<FacturasCabecera> attachedFacturasCabeceraCollection = new ArrayList<FacturasCabecera>();
            for (FacturasCabecera facturasCabeceraCollectionFacturasCabeceraToAttach : cliente.getFacturasCabeceraCollection()) {
                facturasCabeceraCollectionFacturasCabeceraToAttach = em.getReference(facturasCabeceraCollectionFacturasCabeceraToAttach.getClass(), facturasCabeceraCollectionFacturasCabeceraToAttach.getCodigoFactura());
                attachedFacturasCabeceraCollection.add(facturasCabeceraCollectionFacturasCabeceraToAttach);
            }
            cliente.setFacturasCabeceraCollection(attachedFacturasCabeceraCollection);
            em.persist(cliente);
            for (FacturasCabecera facturasCabeceraCollectionFacturasCabecera : cliente.getFacturasCabeceraCollection()) {
                Cliente oldCodigoClienteOfFacturasCabeceraCollectionFacturasCabecera = facturasCabeceraCollectionFacturasCabecera.getCodigoCliente();
                facturasCabeceraCollectionFacturasCabecera.setCodigoCliente(cliente);
                facturasCabeceraCollectionFacturasCabecera = em.merge(facturasCabeceraCollectionFacturasCabecera);
                if (oldCodigoClienteOfFacturasCabeceraCollectionFacturasCabecera != null) {
                    oldCodigoClienteOfFacturasCabeceraCollectionFacturasCabecera.getFacturasCabeceraCollection().remove(facturasCabeceraCollectionFacturasCabecera);
                    oldCodigoClienteOfFacturasCabeceraCollectionFacturasCabecera = em.merge(oldCodigoClienteOfFacturasCabeceraCollectionFacturasCabecera);
                }
            }
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(Cliente cliente) throws IllegalOrphanException, NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            Cliente persistentCliente = em.find(Cliente.class, cliente.getCodigoCliente());
            Collection<FacturasCabecera> facturasCabeceraCollectionOld = persistentCliente.getFacturasCabeceraCollection();
            Collection<FacturasCabecera> facturasCabeceraCollectionNew = cliente.getFacturasCabeceraCollection();
            List<String> illegalOrphanMessages = null;
            for (FacturasCabecera facturasCabeceraCollectionOldFacturasCabecera : facturasCabeceraCollectionOld) {
                if (!facturasCabeceraCollectionNew.contains(facturasCabeceraCollectionOldFacturasCabecera)) {
                    if (illegalOrphanMessages == null) {
                        illegalOrphanMessages = new ArrayList<String>();
                    }
                    illegalOrphanMessages.add("You must retain FacturasCabecera " + facturasCabeceraCollectionOldFacturasCabecera + " since its codigoCliente field is not nullable.");
                }
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            Collection<FacturasCabecera> attachedFacturasCabeceraCollectionNew = new ArrayList<FacturasCabecera>();
            for (FacturasCabecera facturasCabeceraCollectionNewFacturasCabeceraToAttach : facturasCabeceraCollectionNew) {
                facturasCabeceraCollectionNewFacturasCabeceraToAttach = em.getReference(facturasCabeceraCollectionNewFacturasCabeceraToAttach.getClass(), facturasCabeceraCollectionNewFacturasCabeceraToAttach.getCodigoFactura());
                attachedFacturasCabeceraCollectionNew.add(facturasCabeceraCollectionNewFacturasCabeceraToAttach);
            }
            facturasCabeceraCollectionNew = attachedFacturasCabeceraCollectionNew;
            cliente.setFacturasCabeceraCollection(facturasCabeceraCollectionNew);
            cliente = em.merge(cliente);
            for (FacturasCabecera facturasCabeceraCollectionNewFacturasCabecera : facturasCabeceraCollectionNew) {
                if (!facturasCabeceraCollectionOld.contains(facturasCabeceraCollectionNewFacturasCabecera)) {
                    Cliente oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera = facturasCabeceraCollectionNewFacturasCabecera.getCodigoCliente();
                    facturasCabeceraCollectionNewFacturasCabecera.setCodigoCliente(cliente);
                    facturasCabeceraCollectionNewFacturasCabecera = em.merge(facturasCabeceraCollectionNewFacturasCabecera);
                    if (oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera != null && !oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera.equals(cliente)) {
                        oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera.getFacturasCabeceraCollection().remove(facturasCabeceraCollectionNewFacturasCabecera);
                        oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera = em.merge(oldCodigoClienteOfFacturasCabeceraCollectionNewFacturasCabecera);
                    }
                }
            }
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                Integer id = cliente.getCodigoCliente();
                if (findCliente(id) == null) {
                    throw new NonexistentEntityException("The cliente with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(Integer id) throws IllegalOrphanException, NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            Cliente cliente;
            try {
                cliente = em.getReference(Cliente.class, id);
                cliente.getCodigoCliente();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The cliente with id " + id + " no longer exists.", enfe);
            }
            List<String> illegalOrphanMessages = null;
            Collection<FacturasCabecera> facturasCabeceraCollectionOrphanCheck = cliente.getFacturasCabeceraCollection();
            for (FacturasCabecera facturasCabeceraCollectionOrphanCheckFacturasCabecera : facturasCabeceraCollectionOrphanCheck) {
                if (illegalOrphanMessages == null) {
                    illegalOrphanMessages = new ArrayList<String>();
                }
                illegalOrphanMessages.add("This Cliente (" + cliente + ") cannot be destroyed since the FacturasCabecera " + facturasCabeceraCollectionOrphanCheckFacturasCabecera + " in its facturasCabeceraCollection field has a non-nullable codigoCliente field.");
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            em.remove(cliente);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<Cliente> findClienteEntities() {
        return findClienteEntities(true, -1, -1);
    }

    public List<Cliente> findClienteEntities(int maxResults, int firstResult) {
        return findClienteEntities(false, maxResults, firstResult);
    }

    private List<Cliente> findClienteEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(Cliente.class));
            Query q = em.createQuery(cq);
            if (!all) {
                q.setMaxResults(maxResults);
                q.setFirstResult(firstResult);
            }
            return q.getResultList();
        } finally {
            em.close();
        }
    }

    public Cliente findCliente(Integer id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(Cliente.class, id);
        } finally {
            em.close();
        }
    }

    public int getClienteCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<Cliente> rt = cq.from(Cliente.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }
    
}
